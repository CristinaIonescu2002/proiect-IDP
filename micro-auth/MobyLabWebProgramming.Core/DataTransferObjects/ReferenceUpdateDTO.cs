using System.ComponentModel.DataAnnotations;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO for updating an existing Reference. Id is required.
/// Null properties indicate no change. List properties replace existing associations.
/// </summary>
public class ReferenceUpdateDTO
{
    [Required]
    public Guid Id { get; set; }

    [MaxLength(255)]
    public string? Title { get; set; }

    [MaxLength(4095)]
    public string? Description { get; set; } // Use "" empty string to clear

    [MaxLength(255)] // Or handle via file upload
    public string? ImagePath { get; set; }

    /// <summary>
    /// Optional: Provide a new list of Tag Ids to replace existing ones.
    /// An empty list clears associations. Null means don't change associations.
    /// </summary>
    public List<Guid>? TagIds { get; set; }

    // Note: UserId (creator) is typically not updatable.
}