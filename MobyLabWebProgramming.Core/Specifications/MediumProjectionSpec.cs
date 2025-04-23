using Ardalis.Specification;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using System.Linq;

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// Specification to filter and project Medium entities to MediumDTO.
/// </summary>
public sealed class MediumProjectionSpec : Specification<Medium, MediumDTO>
{
    // Implement constructors and .Select() similar to TagProjectionSpec
    /// <summary> Project single Medium by ID. </summary>
    public MediumProjectionSpec(Guid id)
    {
        Query.Where(m => m.Id == id);
        Query.Select(m => new MediumDTO { Id = m.Id, Name = m.Name });
    }

    /// <summary> Project single Medium by Name. </summary>
    // public MediumProjectionSpec(string name)
    // {
    //     Query.Where(m => m.Name.ToLower() == name.ToLower());
    //     Query.Select(m => new MediumDTO { Id = m.Id, Name = m.Name });
    // }

    /// <summary> Projects Medium list with optional search. </summary>
    public MediumProjectionSpec(string? search = null)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            Query.Where(m => m.Name.Contains(search));
        }
        Query.Select(m => new MediumDTO { Id = m.Id, Name = m.Name });
        Query.OrderBy(m => m.Name); // Default sort
    }
}