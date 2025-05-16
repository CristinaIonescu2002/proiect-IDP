using System.Security.Claims; // Added for GetCurrentUser
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests; // Keep for LoginDTO? Or remove if not used elsewhere
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

/// <summary>
/// Service interface for managing User information and related actions like follows and saves.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets detailed information for a specific user.
    /// </summary>
    Task<ServiceResponse<UserDTO>> GetUser(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of users, potentially filtered. Returns UserDTO for now as per example, consider UserSimpleDTO for lists.
    /// </summary>
    Task<ServiceResponse<List<UserDTO>>> GetUsers(string? search = null, CancellationToken cancellationToken = default); // Removed pagination

    /// <summary>
    /// Handles user login, returning user info and a JWT.
    /// </summary>
    Task<ServiceResponse<LoginResponseDTO>> Login(LoginDTO login, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the total count of users.
    /// </summary>
    Task<ServiceResponse<int>> GetUserCount(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new user. Restricted by permissions.
    /// </summary>
    Task<ServiceResponse> AddUser(UserAddDTO user, UserDTO? requestingUser = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing user. Restricted by permissions.
    /// </summary>
    Task<ServiceResponse> UpdateUser(UserUpdateDTO user, UserDTO? requestingUser = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a user. Restricted by permissions.
    /// </summary>
    Task<ServiceResponse> DeleteUser(Guid id, UserDTO? requestingUser = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current logged-in user's information based on claims.
    /// </summary>
    Task<ServiceResponse<UserDTO>> GetCurrentUser(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the list of users the specified user is following.
    /// </summary>
    Task<ServiceResponse<List<UserSimpleDTO>>> GetFollowing(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the list of users who are following the specified user.
    /// </summary>
    Task<ServiceResponse<List<UserSimpleDTO>>> GetFollowers(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Makes the requesting user follow another user.
    /// </summary>
    Task<ServiceResponse> FollowUser(Guid userIdToFollow, UserDTO requestingUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Makes the requesting user unfollow another user.
    /// </summary>
    Task<ServiceResponse> UnfollowUser(Guid userIdToUnfollow, UserDTO requestingUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Makes the requesting user follow a specific tag.
    /// </summary>
    Task<ServiceResponse> FollowTag(Guid tagId, UserDTO requestingUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Makes the requesting user unfollow a specific tag.
    /// </summary>
    Task<ServiceResponse> UnfollowTag(Guid tagId, UserDTO requestingUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the list of tags followed by the specified user.
    /// </summary>
    Task<ServiceResponse<List<TagDTO>>> GetFollowedTags(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Makes the requesting user save a specific reference.
    /// </summary>
    Task<ServiceResponse> SaveReference(Guid referenceId, UserDTO requestingUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Makes the requesting user unsave a specific reference.
    /// </summary>
    Task<ServiceResponse> UnsaveReference(Guid referenceId, UserDTO requestingUser, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the list of references saved by the specified user.
    /// </summary>
    Task<ServiceResponse<List<ReferenceSimpleDTO>>> GetSavedReferences(Guid userId, CancellationToken cancellationToken = default);
}