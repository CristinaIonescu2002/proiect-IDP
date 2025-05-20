namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO for Medium information.
/// </summary>
public class MediumDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    // Optional: Add count if needed
    // public int ArtworkCount { get; set; }
}