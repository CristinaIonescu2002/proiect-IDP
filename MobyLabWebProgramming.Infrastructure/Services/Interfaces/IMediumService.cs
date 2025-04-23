using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests; // If filter DTO needed
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

/// <summary>
/// Service interface for managing Medium entities.
/// </summary>
public interface IMediumService
{
    /// <summary>
    /// Gets information for a specific medium by ID.
    /// </summary>
    Task<ServiceResponse<MediumDTO>> GetMedium(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of media, potentially filtered.
    /// </summary>
    Task<ServiceResponse<List<MediumDTO>>> GetMedia(string? search = null, CancellationToken cancellationToken = default); // Removed pagination

    /// <summary>
    /// Adds a new medium. Typically restricted to Admins.
    /// </summary>
    Task<ServiceResponse> AddMedium(MediumAddDTO medium, UserDTO? requestingUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing medium. Typically restricted to Admins.
    /// </summary>
    Task<ServiceResponse> UpdateMedium(MediumUpdateDTO medium, UserDTO? requestingUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a medium. Typically restricted to Admins and considers usage by artworks.
    /// </summary>
    Task<ServiceResponse> DeleteMedium(Guid id, UserDTO? requestingUser, CancellationToken cancellationToken = default);
}