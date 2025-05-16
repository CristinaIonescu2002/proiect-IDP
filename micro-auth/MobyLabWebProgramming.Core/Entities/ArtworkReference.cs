namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// Join entity for the many-to-many relationship between Artwork and Reference.
/// </summary>
public class ArtworkReference : BaseEntity
{
    /// <summary>
    /// The ID of the artwork.
    /// </summary>
    public Guid ArtworkId { get; set; }

    /// <summary>
    /// The ID of the reference.
    /// </summary>
    public Guid ReferenceId { get; set; }

    /// <summary>
    /// Navigation property for the artwork.
    /// </summary>
    public Artwork Artwork { get; set; } = null!;

    /// <summary>
    /// Navigation property for the reference.
    /// </summary>
    public Reference Reference { get; set; } = null!;
}
