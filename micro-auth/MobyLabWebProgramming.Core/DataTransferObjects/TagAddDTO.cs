using System.ComponentModel.DataAnnotations;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO for adding a new Tag. Tag names should be unique.
/// </summary>
public class TagAddDTO
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = default!;
}