using System.ComponentModel.DataAnnotations;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO for updating an existing Medium. Id is required.
/// </summary>
public class MediumUpdateDTO
{
    [Required]
    public Guid Id { get; set; }

    [Required] // Like Tag, Medium name shouldn't be nullable
    [MaxLength(100)]
    public string Name { get; set; } = default!;
}