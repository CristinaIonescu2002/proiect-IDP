using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class ReferenceSpec : Specification<Reference>
{
    /// <summary> Filter by ID. Includes data needed for updates (Tags). </summary>
    public ReferenceSpec(Guid id)
    {
        Query.Where(e => e.Id == id)
            .Include(r => r.ReferenceTags); // Include join table for updates
    }
    // Add constructors for other filters if needed
}