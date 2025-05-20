namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// Simplified Reference DTO.
/// </summary>
public class ReferenceSimpleDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string ImagePath { get; set; } = default!;
    public UserSimpleDTO User { get; set; } = default!; // User who uploaded it
}