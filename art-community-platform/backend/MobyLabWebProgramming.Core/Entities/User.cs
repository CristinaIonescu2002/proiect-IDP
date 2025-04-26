using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This is an example for a user entity, it will be mapped to a single table and each property will have it's own column except for entity object references also known as navigation properties.
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// The display name of the user.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The unique email address used for login and communication.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// The hashed password for the user account.
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// The role assigned to the user (e.g., Admin, Client). Determines permissions.
    /// </summary>
    public UserRoleEnum Role { get; set; }

    /// <summary>
    /// References to other entities such as this are used to automatically fetch correlated data, this is called a navigation property.
    /// Collection such as this can be used for Many-To-One or Many-To-Many relations.
    /// Note that this field will be null if not explicitly requested via a Include query, also note that the property is used by the ORM, in the database this collection doesn't exist.
    /// </summary>
    public ICollection<UserFile> UserFiles { get; set; } = null!; // Assuming List initialization if used

    /// <summary>
    /// Navigation property referencing the collection of Artworks created by this User.
    /// </summary>
    public ICollection<Artwork> Artworks { get; set; } = new List<Artwork>();

    /// <summary>
    /// Navigation property referencing the collection of References created by this User.
    /// </summary>
    public ICollection<Reference> References { get; set; } = new List<Reference>();

    /// <summary>
    /// Navigation property referencing the collection of join entities linking References saved by this User.
    /// </summary>
    public ICollection<SavedReference> SavedReferences { get; set; } = new List<SavedReference>();

    /// <summary>
    /// Navigation property referencing the collection of join entities linking Tags followed by this User.
    /// </summary>
    public ICollection<UserFollowedTag> FollowedTags { get; set; } = new List<UserFollowedTag>();

    /// <summary>
    /// Navigation property referencing the collection of Users that this User is following.
    /// Represents the "many" side of the User-User following relationship.
    /// </summary>
    public ICollection<User> Following { get; set; } = new List<User>();

    /// <summary>
    /// Navigation property referencing the collection of Users that are following this User.
    /// Represents the "many" side of the User-User follower relationship.
    /// </summary>
    public ICollection<User> Followers { get; set; } = new List<User>();
}