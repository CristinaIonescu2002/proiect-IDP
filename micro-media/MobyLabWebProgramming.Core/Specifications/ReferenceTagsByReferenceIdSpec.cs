using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// Specification to find all ReferenceTag join entities for a specific ReferenceId.
/// Fetches the raw join entity. Used for M2M updates.
/// </summary>
public sealed class ReferenceTagsByReferenceIdSpec : Specification<ReferenceTag>
{
    public ReferenceTagsByReferenceIdSpec(Guid referenceId) =>
        Query.Where(rt => rt.ReferenceId == referenceId);
}