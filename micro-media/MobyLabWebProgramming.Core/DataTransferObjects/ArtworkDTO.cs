namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO for detailed Artwork information.
/// </summary>
public class ArtworkDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime UploadDate { get; set; }
    public string ImagePath { get; set; } = default!;

    // Related data
    public Guid UserId { get; set; } // Foreign key
    public UserSimpleDTO User { get; set; } = default!; // Uploader info

    public Guid MediumId { get; set; } // Foreign key
    public MediumDTO Medium { get; set; } = default!; // Medium info

    /// <summary>
    /// Tags associated with the artwork.
    /// </summary>
    public List<TagDTO> Tags { get; set; } = new();

    /// <summary>
    /// References used in creating the artwork.
    /// </summary>
    public List<ReferenceSimpleDTO> References { get; set; } = new();
}