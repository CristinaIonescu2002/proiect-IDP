using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
// using MobyLabWebProgramming.Core.Requests; // LoginDTO likely not needed if Login handled elsewhere
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
// Corrected namespace for interfaces:
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using MobyLabWebProgramming.Core.Errors;

namespace MobyLabWebProgramming.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UsersController : AuthorizedController
{
    // IUserService injected via base class and accessed via 'UserService' property

    public UsersController(IUserService userService) : base(userService) { }

    #region User Info Actions
    [Authorize]
    [HttpGet("{id:guid}")] // Route: /api/Users/GetById/{id}
    public async Task<ActionResult<RequestResponse<UserDTO>>> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        // Add further auth checks here if necessary
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null) return ErrorMessageResult<UserDTO>(currentUser.Error);
        // Example check:
        // if (currentUser.Result.Role != Core.Enums.UserRoleEnum.Admin && currentUser.Result.Id != id) return Forbid();

        return FromServiceResponse(await UserService.GetUser(id, cancellationToken));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet] // Route: /api/Users/GetList?search=...
    public async Task<ActionResult<RequestResponse<List<UserDTO>>>> GetList([FromQuery] string? search = null, CancellationToken cancellationToken = default)
    {
        // WARNING: UserDTO list is heavy. Consider UserSimpleDTO.
        var currentUser = await GetCurrentUser(); // Verify admin
        return currentUser.Result != null ?
            FromServiceResponse(await UserService.GetUsers(search, cancellationToken)) :
            ErrorMessageResult<List<UserDTO>>(currentUser.Error);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet] // Route: /api/Users/GetCount
     public async Task<ActionResult<RequestResponse<int>>> GetCount(CancellationToken cancellationToken = default)
     {
         var currentUser = await GetCurrentUser();
          return currentUser.Result != null ?
              FromServiceResponse(await UserService.GetUserCount(cancellationToken)) :
              ErrorMessageResult<int>(currentUser.Error);
     }

    [Authorize]
    [HttpGet] // Route: /api/Users/GetCurrent
    public async Task<ActionResult<RequestResponse<UserDTO>>> GetCurrent(CancellationToken cancellationToken = default)
    {
        var currentUserResult = await GetCurrentUser(); // Already gets UserDTO via service
        return currentUserResult.Result != null ?
            FromServiceResponse(currentUserResult) : // Wrap the existing ServiceResponse
            ErrorMessageResult<UserDTO>(currentUserResult.Error);
    }
    #endregion

    #region Registration/Modification Actions
    // Admin adding users
    [Authorize(Roles = "Admin")]
    [HttpPost] // Route: /api/Users/Add
    public async Task<ActionResult<RequestResponse>> Add([FromBody] UserAddDTO user, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if(currentUser.Result == null) return ErrorMessageResult(currentUser.Error);

        // Hash password before sending to service
        user.Password = PasswordUtils.HashPassword(user.Password);

        return FromServiceResponse(await UserService.AddUser(user, currentUser.Result, cancellationToken));
    }

    // Public registration endpoint (if desired, separate from Admin Add)
    // [AllowAnonymous]
    // [HttpPost] // Route: /api/Users/Register
    // public async Task<ActionResult<RequestResponse>> Register([FromBody] UserAddDTO user, CancellationToken cancellationToken = default)
    // {
    //     // Ensure Role in DTO is not Admin if set by client
    //     if(user.Role == Core.Enums.UserRoleEnum.Admin)
    //        return BadRequest(RequestResponse.FromError("Cannot register as Admin."));
    //     user.Password = PasswordUtils.HashPassword(user.Password);
    //     // Pass null for requestingUser, AddUser service should handle this case for initial registration
    //     return FromServiceResponse(await UserService.AddUser(user, null, cancellationToken));
    // }


    [Authorize]
    [HttpPut] // Route: /api/Users/Update
    public async Task<ActionResult<RequestResponse>> Update([FromBody] UserUpdateDTO user, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        if(currentUser.Result == null) return ErrorMessageResult(currentUser.Error);

        string? hashedPassword = !string.IsNullOrWhiteSpace(user.Password) ?
                                PasswordUtils.HashPassword(user.Password) : null;

        return FromServiceResponse(await UserService.UpdateUser(user with { Password = hashedPassword }, currentUser.Result));
    }

    [Authorize]
    [HttpDelete("{id:guid}")] // Route: /api/Users/Delete/{id}
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await UserService.DeleteUser(id, currentUser.Result, cancellationToken)) :
            ErrorMessageResult(currentUser.Error);
    }
    #endregion

    #region Relationship Actions

    [Authorize]
    [HttpPost("{userIdToFollow:guid}")] // Route: /api/Users/Follow/{userIdToFollow}
    public async Task<ActionResult<RequestResponse>> Follow([FromRoute] Guid userIdToFollow, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await UserService.FollowUser(userIdToFollow, currentUser.Result, cancellationToken)) :
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpDelete("{userIdToUnfollow:guid}")] // Route: /api/Users/Unfollow/{userIdToUnfollow}
    public async Task<ActionResult<RequestResponse>> Unfollow([FromRoute] Guid userIdToUnfollow, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await UserService.UnfollowUser(userIdToUnfollow, currentUser.Result, cancellationToken)) :
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize] // Or AllowAnonymous
    [HttpGet("{userId:guid}")] // Route: /api/Users/GetFollowing/{userId}
    public async Task<ActionResult<RequestResponse<List<UserSimpleDTO>>>> GetFollowing([FromRoute] Guid userId, CancellationToken cancellationToken = default)
    {
        return FromServiceResponse(await UserService.GetFollowing(userId, cancellationToken));
    }

    [Authorize] // Or AllowAnonymous
    [HttpGet("{userId:guid}")] // Route: /api/Users/GetFollowers/{userId}
    public async Task<ActionResult<RequestResponse<List<UserSimpleDTO>>>> GetFollowers([FromRoute] Guid userId, CancellationToken cancellationToken = default)
    {
        return FromServiceResponse(await UserService.GetFollowers(userId, cancellationToken));
    }

    [Authorize]
    [HttpPost("{tagId:guid}")] // Route: /api/Users/FollowTag/{tagId}
    public async Task<ActionResult<RequestResponse>> FollowTag([FromRoute] Guid tagId, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await UserService.FollowTag(tagId, currentUser.Result, cancellationToken)) :
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpDelete("{tagId:guid}")] // Route: /api/Users/UnfollowTag/{tagId}
    public async Task<ActionResult<RequestResponse>> UnfollowTag([FromRoute] Guid tagId, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await UserService.UnfollowTag(tagId, currentUser.Result, cancellationToken)) :
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize] // Or AllowAnonymous
    [HttpGet("{userId:guid}")] // Route: /api/Users/GetFollowedTags/{userId}
    public async Task<ActionResult<RequestResponse<List<TagDTO>>>> GetFollowedTags([FromRoute] Guid userId, CancellationToken cancellationToken = default)
    {
         return FromServiceResponse(await UserService.GetFollowedTags(userId, cancellationToken));
    }

    [Authorize]
    [HttpPost("{referenceId:guid}")] // Route: /api/Users/SaveReference/{referenceId}
    public async Task<ActionResult<RequestResponse>> SaveReference([FromRoute] Guid referenceId, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await UserService.SaveReference(referenceId, currentUser.Result, cancellationToken)) :
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpDelete("{referenceId:guid}")] // Route: /api/Users/UnsaveReference/{referenceId}
    public async Task<ActionResult<RequestResponse>> UnsaveReference([FromRoute] Guid referenceId, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await UserService.UnsaveReference(referenceId, currentUser.Result, cancellationToken)) :
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize] // Or AllowAnonymous
    [HttpGet("{userId:guid}")] // Route: /api/Users/GetSavedReferences/{userId}
    public async Task<ActionResult<RequestResponse<List<ReferenceSimpleDTO>>>> GetSavedReferences([FromRoute] Guid userId, CancellationToken cancellationToken = default)
    {
        return FromServiceResponse(await UserService.GetSavedReferences(userId, cancellationToken));
    }

    #endregion
}