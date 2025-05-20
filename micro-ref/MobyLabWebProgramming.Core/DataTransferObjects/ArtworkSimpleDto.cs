namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// Simplified Artwork DTO.
/// </summary>
public class ArtworkSimpleDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string ImagePath { get; set; } = default!;
    public UserSimpleDTO User { get; set; } = default!; // User who created it
}