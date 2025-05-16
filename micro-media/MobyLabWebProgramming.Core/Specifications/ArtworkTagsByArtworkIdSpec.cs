using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// Specification to find all ArtworkTag join entities for a specific ArtworkId.
/// Fetches the raw join entity. Used for M2M updates.
/// </summary>
public sealed class ArtworkTagsByArtworkIdSpec : Specification<ArtworkTag>
{
    public ArtworkTagsByArtworkIdSpec(Guid artworkId) =>
        Query.Where(at => at.ArtworkId == artworkId);
}