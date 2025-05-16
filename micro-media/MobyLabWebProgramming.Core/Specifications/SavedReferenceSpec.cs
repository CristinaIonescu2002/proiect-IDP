using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// Specification to find a specific SavedReference record based on UserId and ReferenceId.
/// Fetches the raw join entity. Used for checking existence or deleting.
/// </summary>
public sealed class SavedReferenceSpec : Specification<SavedReference>, ISingleResultSpecification<SavedReference>
{
    public SavedReferenceSpec(Guid userId, Guid referenceId) =>
        Query.Where(e => e.UserId == userId && e.ReferenceId == referenceId);
}