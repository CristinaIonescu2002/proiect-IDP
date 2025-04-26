namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO for detailed Reference information.
/// </summary>
public class ReferenceDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!; // Assuming you renamed from Name
    public string? Description { get; set; }
    public string ImagePath { get; set; } = default!;

    // Related data
    public Guid UserId { get; set; } // Foreign key
    public UserSimpleDTO User { get; set; } = default!; // Uploader info

    /// <summary>
    /// Tags associated with the reference.
    /// </summary>
    public List<TagDTO> Tags { get; set; } = new();

    /// <summary>
    /// Artworks that have used this reference (Consider pagination if list can be large).
    /// </summary>
    public List<ArtworkSimpleDTO> UsedInArtworks { get; set; } = new();

    // Optional: Count of users who saved this
    // public int SavedByCount { get; set; }
}