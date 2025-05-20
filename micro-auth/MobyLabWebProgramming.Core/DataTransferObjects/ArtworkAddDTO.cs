using System.ComponentModel.DataAnnotations; // Optional for validation attributes

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO for adding a new Artwork.
/// </summary>
public class ArtworkAddDTO
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = default!;

    [MaxLength(4095)]
    public string? Description { get; set; }

    [Required]
    public Guid UserId { get; set; } // Or infer from logged-in user on the server

    [Required]
    public Guid MediumId { get; set; }

    [Required]
    [MaxLength(255)] // Or handle via file upload mechanism
    public string ImagePath { get; set; } = default!;

    /// <summary>
    /// List of Reference Ids to associate with this artwork.
    /// </summary>
    public List<Guid>? ReferenceIds { get; set; } = new();

    /// <summary>
    /// List of Tag Ids to associate with this artwork.
    /// </summary>
    public List<Guid>? TagIds { get; set; } = new();
}