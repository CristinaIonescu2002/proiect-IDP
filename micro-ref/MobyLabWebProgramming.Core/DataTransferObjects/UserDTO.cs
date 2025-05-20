using MobyLabWebProgramming.Core.Enums;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// Detailed User DTO, suitable for a user profile page or detailed view.
/// </summary>
public class UserDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public UserRoleEnum Role { get; set; }

    // Collections representing relationships
    // Note: Populating these fully can be expensive. Consider pagination or separate endpoints.

    /// <summary>
    /// Artworks uploaded by this user (consider using ArtworkSimpleDto or just counts).
    /// </summary>
    public List<ArtworkSimpleDTO> Artworks { get; set; } = new();

    /// <summary>
    /// References uploaded by this user (consider using ReferenceSimpleDto or just counts).
    /// </summary>
    public List<ReferenceSimpleDTO> References { get; set; } = new();

    /// <summary>
    /// References saved by this user.
    /// </summary>
    public List<ReferenceSimpleDTO> SavedReferences { get; set; } = new();

    /// <summary>
    /// Tags followed by this user.
    /// </summary>
    public List<TagDTO> FollowedTags { get; set; } = new();

    /// <summary>
    /// Users that this user is following.
    /// </summary>
    public List<UserSimpleDTO> Following { get; set; } = new();

    /// <summary>
    /// Users that are following this user.
    /// </summary>
    public List<UserSimpleDTO> Followers { get; set; } = new();

    // Optional: Add counts for quick display
    public int ArtworkCount { get; set; }
    public int ReferenceCount { get; set; }
    public int SavedReferenceCount { get; set; }
    public int FollowedTagCount { get; set; }
    public int FollowingCount { get; set; }
    public int FollowerCount { get; set; }
}