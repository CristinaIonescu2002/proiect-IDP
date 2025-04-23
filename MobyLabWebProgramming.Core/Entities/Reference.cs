namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This entity represents a reference material (e.g., inspiration image, pose reference).
/// It can be associated with multiple Artworks and Tags.
/// </summary>
public class Reference : BaseEntity
{
    /// <summary>
    /// The title given to the reference material.
    /// </summary>
    public string Title  { get; set; } = null!;

    /// <summary>
    /// An optional description for the reference material.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The stored path or URL to the image file for this reference.
    /// </summary>
    public string ImagePath { get; set; } = null!;

    /// <summary>
    /// Navigation property referencing the collection of join entities linking Users who saved this Reference.
    /// Represents the "many" side of the User-SavedReference relationship.
    /// </summary>
    public ICollection<SavedReference> SavingUsers { get; set; } = new List<SavedReference>();

    /// <summary>
    /// Foreign key for the User who uploaded/created this Reference.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Navigation property reference to the User who uploaded/created this Reference.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Navigation property referencing the collection of join entities linking Artworks that used this Reference.
    /// Represents the "many" side of the Artwork-ArtworkReference relationship.
    /// </summary>
    public ICollection<ArtworkReference> ArtworkReferences { get; set; } = new List<ArtworkReference>();

    /// <summary>
    /// Navigation property referencing the collection of join entities linking Tags associated with this Reference.
    /// Represents the "many" side of the Reference-ReferenceTag relationship.
    /// </summary>
    public ICollection<ReferenceTag> ReferenceTags { get; set; } = new List<ReferenceTag>();
}