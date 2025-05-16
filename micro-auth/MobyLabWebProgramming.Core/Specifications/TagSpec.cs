using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

public sealed class TagSpec : Specification<Tag>
{
    /// <summary> Filter by ID. </summary>
    public TagSpec(Guid id) => Query.Where(e => e.Id == id);

    /// <summary> Filter by Name (case-insensitive). </summary>
    public TagSpec(string name) => Query.Where(e => e.Name.ToLower() == name.ToLower());
}