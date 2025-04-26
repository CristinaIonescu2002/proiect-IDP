using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// Specification to find all SavedReference records for a specific user,
/// including the related Reference entity and the User who created the Reference.
/// Fetches raw join entities with includes.
/// </summary>
public sealed class SavedReferencesSpec : Specification<SavedReference>
{
    public SavedReferencesSpec(Guid userId) =>
        Query.Where(e => e.UserId == userId)
            .Include(e => e.Reference)        // Include the Reference entity
            .ThenInclude(r => r.User);    // Include the User who created the Reference
}