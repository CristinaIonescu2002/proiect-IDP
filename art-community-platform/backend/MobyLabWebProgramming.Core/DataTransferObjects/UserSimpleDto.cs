namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// Simplified User DTO containing basic identification information.
/// </summary>
public class UserSimpleDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!; // Include email for identification if needed
}