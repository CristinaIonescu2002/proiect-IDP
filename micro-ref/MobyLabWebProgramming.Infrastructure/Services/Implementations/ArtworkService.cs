using System.Net;
using MobyLabWebProgramming.Core.Constants; // For ErrorCodes
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Errors; // For ErrorMessage
using MobyLabWebProgramming.Core.Responses; // Your ServiceResponse
using MobyLabWebProgramming.Core.Specifications;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
// Added IMailService interface reference
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class ArtworkService : IArtworkService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;
    private readonly IMailService _mailService; // Added MailService dependency

    // Updated constructor to inject IMailService
    public ArtworkService(IRepository<WebAppDatabaseContext> repository, IMailService mailService)
    {
        _repository = repository;
        _mailService = mailService; // Assign injected service
    }

    public async Task<ServiceResponse<ArtworkDTO>> GetArtwork(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new ArtworkProjectionSpec(id), cancellationToken);

        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<ArtworkDTO>(new ErrorMessage(HttpStatusCode.NotFound, "Artwork not found.", ErrorCodes.TechnicalError)); // Use specific error code
    }

    public async Task<ServiceResponse<List<ArtworkDTO>>> GetArtworks(
        string? search = null, Guid? userId = null, Guid? mediumId = null, List<Guid>? tagIds = null, CancellationToken cancellationToken = default)
    {
        var spec = new ArtworkProjectionSpec(search, userId, mediumId, tagIds);
        var result = await _repository.ListAsync(spec, cancellationToken);

        return ServiceResponse.ForSuccess(result ?? new List<ArtworkDTO>());
    }

    // Corrected AddArtwork method
    public async Task<ServiceResponse> AddArtwork(ArtworkAddDTO artworkDto, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser == null)
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Unauthorized, "User not identified.", ErrorCodes.TechnicalError)); // Use specific error code

        var mediumExists = (await _repository.GetAsync<Medium>(new MediumSpec(artworkDto.MediumId), cancellationToken)) != null;
        if (!mediumExists)
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.BadRequest, "Specified Medium does not exist.", ErrorCodes.TechnicalError)); // Use specific error code

        // Manual Entity creation
        var newArtwork = new Artwork
        {
            Title = artworkDto.Title,
            Description = artworkDto.Description,
            UserId = requestingUser.Id, // Use requesting user's ID
            MediumId = artworkDto.MediumId,
            ImagePath = artworkDto.ImagePath, // WARNING: Needs secure handling
            ArtworkTags = artworkDto.TagIds?
                          .Distinct()
                          .Select(tagId => new ArtworkTag { TagId = tagId })
                          .ToList() ?? new List<ArtworkTag>(),
            ArtworkReferences = artworkDto.ReferenceIds?
                                .Distinct()
                                .Select(referenceId => new ArtworkReference { ReferenceId = referenceId })
                                .ToList() ?? new List<ArtworkReference>()
        };
        // TODO: Validate TagIds/ReferenceIds exist using GetAsync checks

        // Add the artwork to the database
        await _repository.AddAsync(newArtwork, cancellationToken); // Handles SaveChanges

        var subject = $"Artwork Uploaded: '{newArtwork.Title}'";
        var body = $"Hello {requestingUser.Name},\n\nYour artwork '{newArtwork.Title}' was successfully uploaded to the platform.\n\nThank you!";
        await _mailService.SendMail(requestingUser.Email, subject, body, true, "My App", cancellationToken); // Assuming HTML body is preferred

        // Return success
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateArtwork(ArtworkUpdateDTO artworkDto, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser == null) return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Unauthorized, "User not identified.", ErrorCodes.TechnicalError));

        var entity = await _repository.GetAsync(new ArtworkSpec(artworkDto.Id), cancellationToken);
        if (entity == null)
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Artwork not found.", ErrorCodes.TechnicalError)); // Use specific error code

        if (entity.UserId != requestingUser.Id && requestingUser.Role != UserRoleEnum.Admin)
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Forbidden, "You can only update your own artwork.", ErrorCodes.CannotUpdate));

        entity.Title = artworkDto.Title ?? entity.Title;
        entity.Description = artworkDto.Description ?? entity.Description;
        entity.ImagePath = artworkDto.ImagePath ?? entity.ImagePath;
        if(artworkDto.MediumId.HasValue) {
            var mediumExists = (await _repository.GetAsync<Medium>(new MediumSpec(artworkDto.MediumId.Value), cancellationToken)) != null;
            if (!mediumExists) return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.BadRequest, "Specified Medium does not exist.", ErrorCodes.TechnicalError)); // Use specific error code
            entity.MediumId = artworkDto.MediumId.Value;
        }

        if (artworkDto.TagIds != null) {
            var tagsToRemove = await _repository.ListAsync(new ArtworkTagsByArtworkIdSpec(entity.Id), cancellationToken);
            if(tagsToRemove.Any()) _repository.DbContext.RemoveRange(tagsToRemove);
            entity.ArtworkTags = artworkDto.TagIds.Distinct().Select(tagId => new ArtworkTag { ArtworkId = entity.Id, TagId = tagId }).ToList();
        }
        if (artworkDto.ReferenceIds != null) {
             var refsToRemove = await _repository.ListAsync(new ArtworkReferencesByArtworkIdSpec(entity.Id), cancellationToken);
             if(refsToRemove.Any()) _repository.DbContext.RemoveRange(refsToRemove);
             entity.ArtworkReferences = artworkDto.ReferenceIds.Distinct().Select(referenceId => new ArtworkReference { ArtworkId = entity.Id, ReferenceId = referenceId }).ToList();
        }

        await _repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

     public async Task<ServiceResponse> DeleteArtwork(Guid id, UserDTO? requestingUser = null, CancellationToken cancellationToken = default)
     {
         if (requestingUser == null) return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Unauthorized, "User not identified.", ErrorCodes.TechnicalError));

         var entity = await _repository.GetAsync(new ArtworkSpec(id), cancellationToken);
         if (entity == null)
             return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Artwork not found.", ErrorCodes.TechnicalError)); // Use specific error code

         if (entity.UserId != requestingUser.Id && requestingUser.Role != UserRoleEnum.Admin )
              return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Forbidden, "You can only delete your own artwork.", ErrorCodes.CannotDelete));

         await _repository.DeleteAsync<Artwork>(id, cancellationToken);
         return ServiceResponse.ForSuccess();
     }
}