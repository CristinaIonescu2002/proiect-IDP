namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// Represents an artistic medium (e.g., Painting, Sculpture, Digital Art).
/// </summary>
public class Medium : BaseEntity
{
    /// <summary>
    /// The name of the medium.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Navigation property for the artworks created using this medium.
    /// Represents the "many" side of the one-to-many relationship with Artwork.
    /// </summary>
    public ICollection<Artwork> Artworks { get; set; } = new List<Artwork>();
}