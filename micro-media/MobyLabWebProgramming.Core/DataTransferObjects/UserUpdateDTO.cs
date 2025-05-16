// Change to record class
namespace MobyLabWebProgramming.Core.DataTransferObjects;

/// <summary>
/// This DTO is used to update a user. Id identifies the user.
/// Other properties are nullable; only non-null values sent should be updated.
/// Using a record for immutability features and 'with' expression support.
/// </summary>
public record class UserUpdateDTO(Guid Id) // Use primary constructor for required Id
{
    // Optional properties for update
    public string? Name { get; init; } = null; // 'init' makes them settable only on creation/init
    public string? Password { get; init; } = null;
    // public UserRoleEnum? Role { get; init; } = null; // Add if role updates are needed
}