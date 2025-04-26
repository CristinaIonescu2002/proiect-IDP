using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class ArtworkSpec : Specification<Artwork>
{
    /// <summary> Filter by ID. Includes data needed for updates (Tags/Refs). </summary>
    public ArtworkSpec(Guid id)
    {
        Query.Where(e => e.Id == id)
            .Include(a => a.ArtworkTags) // Include join table for updates
            .Include(a => a.ArtworkReferences); // Include join table for updates
    }
}