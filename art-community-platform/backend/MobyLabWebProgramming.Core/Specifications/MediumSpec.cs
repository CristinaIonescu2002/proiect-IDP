using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class MediumSpec : Specification<Medium>
{
    /// <summary> Filter by ID. </summary>
    public MediumSpec(Guid id) => Query.Where(e => e.Id == id);

    /// <summary> Filter by Name (case-insensitive). </summary>
    public MediumSpec(string name) => Query.Where(e => e.Name.ToLower() == name.ToLower());
}