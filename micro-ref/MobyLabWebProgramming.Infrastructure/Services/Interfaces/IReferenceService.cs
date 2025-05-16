using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests; // If filter DTO needed
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

/// <summary>
/// Service interface for managing Reference entities.
/// </summary>
public interface IReferenceService
{
    /// <summary>
    /// Gets detailed information for a specific reference.
    /// </summary>
    Task<ServiceResponse<ReferenceDTO>> GetReference(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of references, potentially filtered.
    /// </summary>
    Task<ServiceResponse<List<ReferenceDTO>>> GetReferences(string? search = null, Guid? userId = null, List<Guid>? tagIds = null, CancellationToken cancellationToken = default); // Removed pagination

    /// <summary>
    /// Adds a new reference.
    /// </summary>
    Task<ServiceResponse> AddReference(ReferenceAddDTO reference, UserDTO? requestingUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing reference. Restricted by ownership/permissions.
    /// </summary>
    Task<ServiceResponse> UpdateReference(ReferenceUpdateDTO reference, UserDTO? requestingUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a reference. Restricted by ownership/permissions.
    /// </summary>
    Task<ServiceResponse> DeleteReference(Guid id, UserDTO? requestingUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of artworks that have used the specified reference.
    /// </summary>
    Task<ServiceResponse<List<ArtworkSimpleDTO>>> GetArtworksUsingReference(Guid referenceId, CancellationToken cancellationToken = default);
}