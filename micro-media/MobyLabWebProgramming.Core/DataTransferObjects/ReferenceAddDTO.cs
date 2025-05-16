using System.ComponentModel.DataAnnotations;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO for adding a new Reference.
/// </summary>
public class ReferenceAddDTO
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = default!; // Assuming Name was renamed to Title

    [MaxLength(4095)]
    public string? Description { get; set; }

    [Required]
    public Guid UserId { get; set; } // Or infer from logged-in user

    [Required]
    [MaxLength(255)] // Or handle via file upload
    public string ImagePath { get; set; } = default!;

    /// <summary>
    /// List of Tag Ids to associate with this reference.
    /// </summary>
    public List<Guid>? TagIds { get; set; } = new();
}