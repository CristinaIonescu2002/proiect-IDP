using Ardalis.Specification;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using System.Linq;

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// Specification to filter and project Tag entities to TagDTO.
/// </summary>
public sealed class TagProjectionSpec : Specification<Tag, TagDTO>
{
    /// <summary> Project single Tag by ID. </summary>
    public TagProjectionSpec(Guid id)
    {
        Query.Where(t => t.Id == id);
        // Define the projection (Select clause)
        Query.Select(t => new TagDTO { Id = t.Id, Name = t.Name });
    }

    // REMOVED this constructor to resolve ambiguity:
    // /// <summary> Project single Tag by Name. </summary>
    // public TagProjectionSpec(string name)
    // {
    //     Query.Where(t => t.Name.ToLower() == name.ToLower());
    //     Query.Select(t => new TagDTO { Id = t.Id, Name = t.Name });
    // }

    /// <summary> Projects Tag list with optional search. </summary>
    public TagProjectionSpec(string? search = null)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            // Using OrdinalIgnoreCase for case-insensitive comparison is generally better for performance than ToLower()
            Query.Where(t => t.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        }
        // Define the projection (Select clause)
        Query.Select(t => new TagDTO { Id = t.Id, Name = t.Name });
        Query.OrderBy(t => t.Name); // Default sort
    }
}