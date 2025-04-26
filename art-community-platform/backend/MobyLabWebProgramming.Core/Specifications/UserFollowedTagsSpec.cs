using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// Specification to find all UserFollowedTag records for a specific user,
/// including the related Tag entity. Fetches raw join entities with includes.
/// </summary>
public sealed class UserFollowedTagsSpec : Specification<UserFollowedTag>
{
    public UserFollowedTagsSpec(Guid userId) =>
        Query.Where(e => e.UserId == userId)
            .Include(e => e.Tag); // Include the Tag entity
}