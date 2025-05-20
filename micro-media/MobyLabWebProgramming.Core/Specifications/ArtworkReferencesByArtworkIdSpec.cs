using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// Specification to find all ArtworkReference join entities for a specific ArtworkId.
/// Fetches the raw join entity. Used for M2M updates.
/// </summary>
public sealed class ArtworkReferencesByArtworkIdSpec : Specification<ArtworkReference>
{
    public ArtworkReferencesByArtworkIdSpec(Guid artworkId) =>
        Query.Where(ar => ar.ArtworkId == artworkId);
}