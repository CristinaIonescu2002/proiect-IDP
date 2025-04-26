namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// DTO for Tag information.
/// </summary>
public class TagDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    // Optional: Add counts if frequently needed and performant to calculate
    // public int ArtworkCount { get; set; }
    // public int ReferenceCount { get; set; }
    // public int FollowerCount { get; set; }
}