namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// Join entity for the many-to-many relationship between Artwork and Tag.
/// </summary>
public class ArtworkTag : BaseEntity
{
    /// <summary>
    /// The ID of the artwork.
    /// </summary>
    public Guid ArtworkId { get; set; }

    /// <summary>
    /// The ID of the tag.
    /// </summary>
    public Guid TagId { get; set; }

    /// <summary>
    /// Navigation property for the artwork.
    /// </summary>
    public Artwork Artwork { get; set; } = null!;

    /// <summary>
    /// Navigation property for the tag.
    /// </summary>
    public Tag Tag { get; set; } = null!;
}