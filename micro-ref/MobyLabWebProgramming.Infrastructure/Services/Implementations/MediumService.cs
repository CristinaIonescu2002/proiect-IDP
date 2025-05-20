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
using System.Linq;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class MediumService : IMediumService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;

    public MediumService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }

     public async Task<ServiceResponse<MediumDTO>> GetMedium(Guid id, CancellationToken cancellationToken = default)
     {
         var result = await _repository.GetAsync(new MediumProjectionSpec(id), cancellationToken);
         return result != null ?
             ServiceResponse.ForSuccess(result) :
             ServiceResponse.FromError<MediumDTO>(new ErrorMessage(HttpStatusCode.NotFound, "Medium not found.", ErrorCodes.TechnicalError));
     }

     public async Task<ServiceResponse<List<MediumDTO>>> GetMedia(string? search = null, CancellationToken cancellationToken = default)
     {
         var spec = new MediumProjectionSpec(search);
         var result = await _repository.ListAsync(spec, cancellationToken);
         return ServiceResponse.ForSuccess(result ?? new List<MediumDTO>());
     }

     public async Task<ServiceResponse> AddMedium(MediumAddDTO mediumAddDto, UserDTO? requestingUser, CancellationToken cancellationToken = default)
     {
         if (requestingUser == null || requestingUser.Role != UserRoleEnum.Admin)
             return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Forbidden, "Only Admins can add media.", ErrorCodes.CannotAdd));

         var existing = await _repository.GetAsync(new MediumSpec(mediumAddDto.Name), cancellationToken);
         if (existing != null)
             return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Conflict, "Medium name already exists.", ErrorCodes.TechnicalError));

         var medium = new Medium { Name = mediumAddDto.Name };
         await _repository.AddAsync(medium, cancellationToken);
         return ServiceResponse.ForSuccess();
     }

     public async Task<ServiceResponse> UpdateMedium(MediumUpdateDTO mediumUpdateDto, UserDTO? requestingUser, CancellationToken cancellationToken = default)
     {
         if (requestingUser == null || requestingUser.Role != UserRoleEnum.Admin)
             return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Forbidden, "Only Admins can update media.", ErrorCodes.CannotUpdate));

         var medium = await _repository.GetAsync(new MediumSpec(mediumUpdateDto.Id), cancellationToken);
         if (medium == null)
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Medium not found.", ErrorCodes.TechnicalError));

         if (!string.IsNullOrWhiteSpace(mediumUpdateDto.Name) && !medium.Name.Equals(mediumUpdateDto.Name, StringComparison.OrdinalIgnoreCase))
         {
              var existingConflict = await _repository.GetAsync(new MediumSpec(mediumUpdateDto.Name), cancellationToken);
              if (existingConflict != null && existingConflict.Id != medium.Id)
                  return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Conflict, "New medium name already exists.", ErrorCodes.TechnicalError));
         }

         medium.Name = mediumUpdateDto.Name ?? medium.Name;
         await _repository.UpdateAsync(medium, cancellationToken);
         return ServiceResponse.ForSuccess();
     }

     public async Task<ServiceResponse> DeleteMedium(Guid id, UserDTO? requestingUser, CancellationToken cancellationToken = default)
     {
         if (requestingUser == null || requestingUser.Role != UserRoleEnum.Admin)
             return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Forbidden, "Only Admins can delete media.", ErrorCodes.CannotDelete));

         var medium = await _repository.GetAsync(new MediumSpec(id), cancellationToken);
         if (medium == null)
              return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Medium not found.", ErrorCodes.TechnicalError));

         var usageCount = await _repository.GetCountAsync(new ArtworkByMediumIdSpec(id), cancellationToken);
         if(usageCount > 0)
              return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Conflict, "Cannot delete medium as it is currently associated with artworks.", ErrorCodes.TechnicalError));

         await _repository.DeleteAsync<Medium>(id, cancellationToken);
         return ServiceResponse.ForSuccess();
     }
}