using System.Net;
using MobyLabWebProgramming.Core.Constants;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Specifications;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class ReferenceService : IReferenceService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;

    public ReferenceService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<ReferenceDTO>> GetReference(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new ReferenceProjectionSpec(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<ReferenceDTO>(new ErrorMessage(HttpStatusCode.NotFound, "Reference not found.", ErrorCodes.TechnicalError));
    }

    public async Task<ServiceResponse<List<ReferenceDTO>>> GetReferences(
        string? search = null, Guid? userId = null, List<Guid>? tagIds = null, CancellationToken cancellationToken = default)
    {
        var spec = new ReferenceProjectionSpec(search, userId, tagIds);
        var result = await _repository.ListAsync(spec, cancellationToken);
        return ServiceResponse.ForSuccess(result ?? new List<ReferenceDTO>());
    }

    public async Task<ServiceResponse> AddReference(ReferenceAddDTO referenceDto, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser == null)
             return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Unauthorized, "User not identified.", ErrorCodes.TechnicalError));

        var newReference = new Reference
        {
            Title = referenceDto.Title,
            Description = referenceDto.Description,
            UserId = requestingUser.Id,
            ImagePath = referenceDto.ImagePath,
            ReferenceTags = referenceDto.TagIds?
                            .Distinct()
                            .Select(tagId => new ReferenceTag { TagId = tagId })
                            .ToList() ?? new List<ReferenceTag>()
        };
        // TODO: Validate TagIds exist

        await _repository.AddAsync(newReference, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateReference(ReferenceUpdateDTO referenceDto, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser == null) return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Unauthorized, "User not identified.", ErrorCodes.TechnicalError));

        var entity = await _repository.GetAsync(new ReferenceSpec(referenceDto.Id), cancellationToken);
        if (entity == null)
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Reference not found.", ErrorCodes.TechnicalError));

        if (entity.UserId != requestingUser.Id && requestingUser.Role != UserRoleEnum.Admin)
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Forbidden, "You can only update your own reference.", ErrorCodes.CannotUpdate));

        entity.Title = referenceDto.Title ?? entity.Title;
        entity.Description = referenceDto.Description ?? entity.Description;
        entity.ImagePath = referenceDto.ImagePath ?? entity.ImagePath;

        if (referenceDto.TagIds != null)
        {
            // TODO: Validate TagIds exist
            var tagsToRemove = await _repository.ListAsync(new ReferenceTagsByReferenceIdSpec(entity.Id), cancellationToken);
            if(tagsToRemove.Any()) _repository.DbContext.RemoveRange(tagsToRemove);
            entity.ReferenceTags = referenceDto.TagIds.Distinct().Select(tagId => new ReferenceTag { ReferenceId = entity.Id, TagId = tagId }).ToList();
        }

        await _repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

     public async Task<ServiceResponse> DeleteReference(Guid id, UserDTO? requestingUser = null, CancellationToken cancellationToken = default)
     {
         if (requestingUser == null) return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Unauthorized, "User not identified.", ErrorCodes.TechnicalError));

         var entity = await _repository.GetAsync(new ReferenceSpec(id), cancellationToken);
         if (entity == null)
             return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Reference not found.", ErrorCodes.TechnicalError));

         if (entity.UserId != requestingUser.Id && requestingUser.Role != UserRoleEnum.Admin)
             return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Forbidden, "You can only delete your own reference.", ErrorCodes.CannotDelete));

         await _repository.DeleteAsync<Reference>(id, cancellationToken);
         return ServiceResponse.ForSuccess();
     }

     public async Task<ServiceResponse<List<ArtworkSimpleDTO>>> GetArtworksUsingReference(Guid referenceId, CancellationToken cancellationToken = default)
     {
         var spec = new ArtworksUsingReferenceProjectionSpec(referenceId);
         var result = await _repository.ListAsync(spec, cancellationToken);
         return ServiceResponse.ForSuccess(result ?? new List<ArtworkSimpleDTO>());
     }
}