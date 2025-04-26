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

public class TagService : ITagService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;

    public TagService(IRepository<WebAppDatabaseContext> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<TagDTO>> GetTag(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new TagProjectionSpec(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<TagDTO>(new ErrorMessage(HttpStatusCode.NotFound, "Tag not found.", ErrorCodes.TechnicalError));
    }

    public async Task<ServiceResponse<TagDTO>> GetTagByName(string name, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new TagProjectionSpec(name), cancellationToken);
        return result != null ?
           ServiceResponse.ForSuccess(result) :
           ServiceResponse.FromError<TagDTO>(new ErrorMessage(HttpStatusCode.NotFound, "Tag not found.", ErrorCodes.TechnicalError));
    }


    public async Task<ServiceResponse<List<TagDTO>>> GetTags(string? search = null, CancellationToken cancellationToken = default)
    {
        var spec = new TagProjectionSpec(search);
        var result = await _repository.ListAsync(spec, cancellationToken);
        return ServiceResponse.ForSuccess(result ?? new List<TagDTO>());
    }

    public async Task<ServiceResponse> AddTag(TagAddDTO tagAddDto, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetAsync(new TagSpec(tagAddDto.Name), cancellationToken);
        if (existing != null)
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Conflict, "Tag name already exists.", ErrorCodes.TechnicalError));

        var tag = new Tag { Name = tagAddDto.Name };
        await _repository.AddAsync(tag, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateTag(TagUpdateDTO tagUpdateDto, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser == null || requestingUser.Role != UserRoleEnum.Admin)
             return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Forbidden, "Only Admins can update tags.", ErrorCodes.CannotUpdate));

        var tag = await _repository.GetAsync(new TagSpec(tagUpdateDto.Id), cancellationToken);
        if (tag == null)
             return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Tag not found.", ErrorCodes.TechnicalError));

        if (!string.IsNullOrWhiteSpace(tagUpdateDto.Name) && !tag.Name.Equals(tagUpdateDto.Name, StringComparison.OrdinalIgnoreCase))
        {
             var existingConflict = await _repository.GetAsync(new TagSpec(tagUpdateDto.Name), cancellationToken);
             if (existingConflict != null && existingConflict.Id != tag.Id)
                 return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Conflict, "New tag name already exists.", ErrorCodes.TechnicalError));
        }

        tag.Name = tagUpdateDto.Name ?? tag.Name;
        await _repository.UpdateAsync(tag, cancellationToken);
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteTag(Guid id, UserDTO? requestingUser = null, CancellationToken cancellationToken = default)
    {
        if (requestingUser == null || requestingUser.Role != UserRoleEnum.Admin)
             return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Forbidden, "Only Admins can delete tags.", ErrorCodes.CannotDelete));

        var tag = await _repository.GetAsync(new TagSpec(id), cancellationToken);
        if (tag == null)
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Tag not found.", ErrorCodes.TechnicalError));

        // Consider check if tag is in use
        // var usageCount = await _repository.GetCountAsync(...)
        // if (usageCount > 0) return ServiceResponse.FromError(...)

        await _repository.DeleteAsync<Tag>(id, cancellationToken);
        return ServiceResponse.ForSuccess();
    }
}