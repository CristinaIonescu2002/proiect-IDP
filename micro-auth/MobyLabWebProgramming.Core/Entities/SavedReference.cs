namespace MobyLabWebProgramming.Core.Entities;

/// <summary>
/// This entity represents the many-to-many join entity between Users and References they have saved.
/// Links a specific User to a specific Reference.
/// </summary>
public class SavedReference : BaseEntity
{
    /// <summary>
    /// Foreign key for the User who saved the reference.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Navigation property reference to the User who saved the reference.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Foreign key for the Reference that was saved.
    /// </summary>
    public Guid ReferenceId { get; set; }

    /// <summary>
    /// Navigation property reference to the Reference that was saved.
    /// </summary>
    public Reference Reference { get; set; } = null!;
}