using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests; // If filter DTO needed
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

/// <summary>
/// Service interface for managing Artwork entities.
/// </summary>
public interface IArtworkService
{
    /// <summary>
    /// Gets detailed information for a specific artwork.
    /// </summary>
    Task<ServiceResponse<ArtworkDTO>> GetArtwork(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of artworks, potentially filtered.
    /// </summary>
    Task<ServiceResponse<List<ArtworkDTO>>> GetArtworks(string? search = null, Guid? userId = null, Guid? mediumId = null, List<Guid>? tagIds = null, CancellationToken cancellationToken = default); // Removed pagination

    /// <summary>
    /// Adds a new artwork.
    /// </summary>
    Task<ServiceResponse> AddArtwork(ArtworkAddDTO artwork, UserDTO? requestingUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing artwork. Restricted by ownership/permissions.
    /// </summary>
    Task<ServiceResponse> UpdateArtwork(ArtworkUpdateDTO artwork, UserDTO? requestingUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an artwork. Restricted by ownership/permissions.
    /// </summary>
    Task<ServiceResponse> DeleteArtwork(Guid id, UserDTO? requestingUser, CancellationToken cancellationToken = default);
}