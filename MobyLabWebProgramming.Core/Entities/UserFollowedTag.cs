namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This entity represents the many-to-many join entity between Users and Tags they follow.
/// Links a specific User to a specific Tag.
/// </summary>
public class UserFollowedTag : BaseEntity
{
    /// <summary>
    /// Foreign key for the User who is following the tag.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Navigation property reference to the User who is following the tag.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Foreign key for the Tag being followed.
    /// </summary>
    public Guid TagId { get; set; }

    /// <summary>
    /// Navigation property reference to the Tag being followed.
    /// </summary>
    public Tag Tag { get; set; } = null!;
}