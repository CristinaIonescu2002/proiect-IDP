using System.ComponentModel.DataAnnotations; // Optional for validation attributes

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO for updating an existing Artwork. Id is required.
/// Null properties indicate no change is requested for that field.
/// For collections (ReferenceIds, TagIds), providing a list typically replaces the existing associations.
/// </summary>
public class ArtworkUpdateDTO
{
    [Required]
    public Guid Id { get; set; }

    [MaxLength(255)]
    public string? Title { get; set; }

    [MaxLength(4095)]
    public string? Description { get; set; } // Use "" empty string to clear description if needed

    public Guid? MediumId { get; set; }

    [MaxLength(255)] // Or handle via file upload mechanism
    public string? ImagePath { get; set; }

    /// <summary>
    /// Optional: Provide a new list of Reference Ids to replace existing ones.
    /// An empty list clears associations. Null means don't change associations.
    /// </summary>
    public List<Guid>? ReferenceIds { get; set; }

    /// <summary>
    /// Optional: Provide a new list of Tag Ids to replace existing ones.
    /// An empty list clears associations. Null means don't change associations.
    /// </summary>
    public List<Guid>? TagIds { get; set; }

    // Note: UserId (creator) is typically not updatable.
}