using System.ComponentModel.DataAnnotations;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO for adding a new Medium. Medium names should be unique.
/// </summary>
public class MediumAddDTO
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = default!;
}