using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests; // If filter DTO needed
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

/// <summary>
/// Service interface for managing Tag entities.
/// </summary>
public interface ITagService
{
    /// <summary>
    /// Gets information for a specific tag by ID.
    /// </summary>
    Task<ServiceResponse<TagDTO>> GetTag(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information for a specific tag by name.
    /// </summary>
    Task<ServiceResponse<TagDTO>> GetTagByName(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of tags, potentially filtered.
    /// </summary>
    Task<ServiceResponse<List<TagDTO>>> GetTags(string? search = null, CancellationToken cancellationToken = default); // Removed pagination

    /// <summary>
    /// Adds a new tag. May require permissions.
    /// </summary>
    Task<ServiceResponse> AddTag(TagAddDTO tag, UserDTO? requestingUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing tag. May require permissions.
    /// </summary>
    Task<ServiceResponse> UpdateTag(TagUpdateDTO tag, UserDTO? requestingUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a tag. May require permissions and consider implications.
    /// </summary>
    Task<ServiceResponse> DeleteTag(Guid id, UserDTO? requestingUser, CancellationToken cancellationToken = default);
}