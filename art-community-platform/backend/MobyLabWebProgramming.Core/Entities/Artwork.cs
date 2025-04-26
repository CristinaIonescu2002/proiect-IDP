using System; // Required for Guid
using System.Collections.Generic; // Required for ICollection

namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// Represents a single piece of art uploaded to the platform.
/// </summary>
public class Artwork : BaseEntity
{
    /// <summary>
    /// The title of the artwork.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// A description of the artwork.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The date and time the artwork was uploaded.
    /// </summary>
    public DateTime UploadDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The ID of the user who uploaded the artwork.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The path to the image file of the artwork.
    /// </summary>
    public string ImagePath { get; set; } = null!;

    /// <summary>
    /// Foreign key referencing the Medium of this artwork.
    /// </summary>
    public Guid MediumId { get; set; }

    /// <summary>
    /// Navigation property for the Medium of this artwork.
    /// Represents the "one" side of the one-to-many relationship with Medium.
    /// </summary>
    public Medium Medium { get; set; } = null!;

    /// <summary>
    /// Navigation property for the user who uploaded the artwork.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Navigation property for the references used in the artwork.
    /// </summary>
    // Initialized the collection as previously recommended
    public ICollection<ArtworkReference> ArtworkReferences { get; set; } = new List<ArtworkReference>();

    /// <summary>
    /// Navigation property for the tags associated with the artwork.
    /// </summary>
    // Initialized the collection as previously recommended
    public ICollection<ArtworkTag> ArtworkTags { get; set; } = new List<ArtworkTag>();
}