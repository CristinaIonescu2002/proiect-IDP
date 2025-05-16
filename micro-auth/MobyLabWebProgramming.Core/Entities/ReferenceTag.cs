namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// Join entity for the many-to-many relationship between Reference and Tag.
/// </summary>
public class ReferenceTag : BaseEntity
{
    /// <summary>
    /// The ID of the Reference.
    /// </summary>
    public Guid ReferenceId { get; set; }

    /// <summary>
    /// The ID of the tag.
    /// </summary>
    public Guid TagId { get; set; }

    /// <summary>
    /// Navigation property for the Reference.
    /// </summary>
    public Reference Reference { get; set; } = null!;

    /// <summary>
    /// Navigation property for the tag.
    /// </summary>
    public Tag Tag { get; set; } = null!;
}