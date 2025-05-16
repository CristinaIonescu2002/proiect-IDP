using System.ComponentModel.DataAnnotations;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO for updating an existing Tag. Id is required.
/// </summary>
public class TagUpdateDTO
{
    [Required]
    public Guid Id { get; set; }

    [Required] // Tag name likely shouldn't be nullable, update requires a value
    [MaxLength(255)]
    public string Name { get; set; } = default!;
}