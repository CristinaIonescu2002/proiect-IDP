namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This entity represents a tag or keyword that can be associated with Artworks and References,
/// and followed by Users.
/// </summary>
public class Tag : BaseEntity
{
    /// <summary>
    /// The name of the tag (e.g., "fantasy", "character design"). Should be unique.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Navigation property referencing the collection of join entities linking Artworks associated with this Tag.
    /// </summary>
    public ICollection<ArtworkTag> ArtworkTags { get; set; } = new List<ArtworkTag>();

    /// <summary>
    /// Navigation property referencing the collection of join entities linking References associated with this Tag.
    /// </summary>
    public ICollection<ReferenceTag> ReferenceTags { get; set; } = new List<ReferenceTag>();

    /// <summary>
    /// Navigation property referencing the collection of join entities linking Users following this Tag.
    /// </summary>
    public ICollection<UserFollowedTag> FollowingUsers { get; set; } = new List<UserFollowedTag>();
}