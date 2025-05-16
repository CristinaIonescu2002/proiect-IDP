using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// Specification to filter Artwork entities by MediumId.
/// Fetches the raw entity. Can be used with GetCountAsync or ListAsync(spec.Take(1)).
/// </summary>
public sealed class ArtworkByMediumIdSpec : Specification<Artwork>
{
    public ArtworkByMediumIdSpec(Guid mediumId) =>
        Query.Where(a => a.MediumId == mediumId);
}