using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// Specification to find a specific UserFollowedTag record based on UserId and TagId.
/// Fetches the raw join entity. Used for checking existence or deleting.
/// </summary>
public sealed class UserFollowedTagSpec : Specification<UserFollowedTag>, ISingleResultSpecification<UserFollowedTag>
{
    public UserFollowedTagSpec(Guid userId, Guid tagId) =>
        Query.Where(e => e.UserId == userId && e.TagId == tagId);
}