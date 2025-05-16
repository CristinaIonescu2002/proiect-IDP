using MobyLabWebProgramming.Core.Entities;
using Ardalis.Specification;

namespace MobyLabWebProgramming.Core.Specifications;

/// <summary>
/// Specification to filter User entities. Fetches the raw entity.
/// Includes chainable methods for common relationship includes.
/// </summary>
public sealed class UserSpec : Specification<User>
{
    /// <summary> Filter by ID. </summary>
    public UserSpec(Guid id) => Query.Where(e => e.Id == id);

    /// <summary> Filter by Email. </summary>
    public UserSpec(string email) => Query.Where(e => e.Email.ToLower() == email.ToLower()); // Ensure case-insensitivity if needed

    /// <summary> Includes the Following collection. </summary>
    public UserSpec IncludeFollows()
    {
        Query.Include(u => u.Following);
        return this;
    }

    /// <summary> Includes the Followers collection. </summary>
    public UserSpec IncludeFollowers()
    {
        Query.Include(u => u.Followers);
        return this;
    }

    /// <summary> Includes collections often needed for detailed views or mapping. </summary>
    public UserSpec IncludeAllForProfile()
    {
        Query.Include(u => u.Artworks) // WARNING: Can be large
            // .Include(u => u.References) // WARNING: Can be large
            .Include(u => u.Following)
            .Include(u => u.Followers)
            // Include join tables/related entities if needed for mapping complex UserDTO
            .Include(u => u.SavedReferences).ThenInclude(sr => sr.Reference) // Include Saved -> Reference
            .Include(u => u.FollowedTags).ThenInclude(uft => uft.Tag);      // Include Followed -> Tag
        return this;
    }
}