using System.Net;
using System.Security.Claims;
using MobyLabWebProgramming.Core.Constants;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Specifications;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public partial class UserService : IUserService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;
    private readonly ILoginService _loginService;
    private readonly IMailService _mailService;

     public UserService(IRepository<WebAppDatabaseContext> repository,
                        ILoginService loginService,
                        IMailService mailService)
     {
         _repository = repository;
         _loginService = loginService;
         _mailService = mailService;
     }

    public async Task<ServiceResponse<UserDTO>> GetUser(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new UserProjectionSpec(id), cancellationToken);
        return result != null ?
            ServiceResponse.ForSuccess(result) :
            ServiceResponse.FromError<UserDTO>(new ErrorMessage(HttpStatusCode.NotFound,"User not found.", ErrorCodes.TechnicalError)); // Use new factory method
    }

    public async Task<ServiceResponse<List<UserDTO>>> GetUsers(string? search = null, CancellationToken cancellationToken = default)
    {
        var spec = new UserProjectionSpec(search);
        var result = await _repository.ListAsync(spec, cancellationToken);
        return ServiceResponse.ForSuccess(result ?? new List<UserDTO>()); // Use new factory method
    }

    public async Task<ServiceResponse<LoginResponseDTO>> Login(LoginDTO login, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAsync(new UserSpec(login.Email), cancellationToken);
        if (result == null)
            return ServiceResponse.FromError<LoginResponseDTO>(new ErrorMessage(HttpStatusCode.NotFound,"User not found.", ErrorCodes.TechnicalError)); // Use new factory method

        if (result.Password != login.Password)
            return ServiceResponse.FromError<LoginResponseDTO>(new ErrorMessage(HttpStatusCode.BadRequest, "Wrong password!", ErrorCodes.WrongPassword)); // Use new factory method

        var user = new UserDTO { Id = result.Id, Email = result.Email, Name = result.Name, Role = result.Role };
        return ServiceResponse.ForSuccess(new LoginResponseDTO { User = user, Token = _loginService.GetToken(user, DateTime.UtcNow, new(7, 0, 0, 0)) }); // Use new factory method
    }

     public async Task<ServiceResponse<int>> GetUserCount(CancellationToken cancellationToken = default) =>
         ServiceResponse.ForSuccess(await _repository.GetCountAsync<User>(cancellationToken)); // Use new factory method

    public async Task<ServiceResponse> AddUser(UserAddDTO user, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        long userCount = await _repository.GetCountAsync<User>(cancellationToken);
        if (userCount > 0 && (requestingUser == null || requestingUser.Role != UserRoleEnum.Admin))
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Forbidden, "Only the admin can add users!", ErrorCodes.CannotAdd));

        var result = await _repository.GetAsync(new UserSpec(user.Email), cancellationToken);
        if (result != null)
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Conflict, "The user already exists!", ErrorCodes.UserAlreadyExists));

        // WARNING: Storing plain text password - HASH PASSWORDS!
        var newUser = new User { Email = user.Email, Name = user.Name, Role = user.Role, Password = user.Password };
        await _repository.AddAsync(newUser, cancellationToken);
        await _mailService.SendMail(user.Email, "Welcome!", MailTemplates.UserAddTemplate(user.Name), true, "My App", cancellationToken);
        return ServiceResponse.ForSuccess(); // Use new factory method
    }

    public async Task<ServiceResponse> UpdateUser(UserUpdateDTO user, UserDTO? requestingUser, CancellationToken cancellationToken = default)
    {
        if (requestingUser == null) return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Unauthorized, "User not identified.", ErrorCodes.TechnicalError));
        if (requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != user.Id)
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Forbidden, "Only the admin or the own user can update the user!", ErrorCodes.CannotUpdate));

        var entity = await _repository.GetAsync(new UserSpec(user.Id), cancellationToken);
        if (entity == null)
             return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "User not found.", ErrorCodes.TechnicalError));

        entity.Name = user.Name ?? entity.Name;
        // WARNING: Storing plain text password - HASH PASSWORDS!
        if(!string.IsNullOrWhiteSpace(user.Password)) { entity.Password = user.Password; }

        await _repository.UpdateAsync(entity, cancellationToken);
        return ServiceResponse.ForSuccess(); // Use new factory method
    }

    public async Task<ServiceResponse> DeleteUser(Guid id, UserDTO? requestingUser = null, CancellationToken cancellationToken = default)
    {
         if (requestingUser == null) return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Unauthorized, "User not identified.", ErrorCodes.TechnicalError));
         if (requestingUser.Role != UserRoleEnum.Admin && requestingUser.Id != id)
             return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.Forbidden, "Only the admin or the own user can delete the user!", ErrorCodes.CannotDelete));

         var entity = await _repository.GetAsync<User>(id, cancellationToken);
         if (entity == null)
             return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "User not found.", ErrorCodes.TechnicalError));

         var result = await _repository.DeleteAsync<User>(id, cancellationToken);
         return result > 0 ? ServiceResponse.ForSuccess() // Use new factory method
                           : ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.InternalServerError, "Delete failed.", ErrorCodes.CannotDelete));
    }

    // Helper remains same
    private async Task<User?> GetUserEntityFromClaims(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken = default)
    {
        var userIdClaim = userClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (Guid.TryParse(userIdClaim, out var userId)) { return await _repository.GetAsync(new UserSpec(userId).IncludeAllForProfile(), cancellationToken); }
        return null;
    }

    // GetCurrentUser: Use new ServiceResponse factory
    public async Task<ServiceResponse<UserDTO>> GetCurrentUser(ClaimsPrincipal userClaimsPrincipal, CancellationToken cancellationToken = default)
    {
        var userEntity = await GetUserEntityFromClaims(userClaimsPrincipal, cancellationToken);
        if (userEntity == null)
            return ServiceResponse.FromError<UserDTO>(new ErrorMessage(HttpStatusCode.NotFound,"User not found.", ErrorCodes.TechnicalError)); // Use new factory method

        var userDto = new UserDTO { Id = userEntity.Id, Name = userEntity.Name, Email = userEntity.Email, Role = userEntity.Role, /* etc. */ };
        return ServiceResponse.ForSuccess(userDto); // Use new factory method
    }


     // --- Relationship Management (Using new ServiceResponse) ---

     public async Task<ServiceResponse> FollowUser(Guid userIdToFollow, UserDTO requestingUser, CancellationToken cancellationToken = default)
     {
          var follower = await _repository.GetAsync(new UserSpec(requestingUser.Id).IncludeFollows(), cancellationToken);
          var userToFollow = await _repository.GetAsync(new UserSpec(userIdToFollow), cancellationToken);
          if (follower == null || userToFollow == null) return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "User not found.", ErrorCodes.TechnicalError));
          if (follower.Id == userToFollow.Id) return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.BadRequest, "Cannot follow yourself.", ErrorCodes.TechnicalError));
          if (!follower.Following.Any(f => f.Id == userIdToFollow)) {
              follower.Following.Add(userToFollow);
              await _repository.UpdateAsync(follower, cancellationToken);
          }
          return ServiceResponse.ForSuccess();
     }

     public async Task<ServiceResponse> UnfollowUser(Guid userIdToUnfollow, UserDTO requestingUser, CancellationToken cancellationToken = default)
     {
          var follower = await _repository.GetAsync(new UserSpec(requestingUser.Id).IncludeFollows(), cancellationToken);
          var userToUnfollow = follower?.Following.FirstOrDefault(f => f.Id == userIdToUnfollow);
          if (follower == null) return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "User not found.", ErrorCodes.TechnicalError));
          if (userToUnfollow == null) return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.BadRequest, "Not following this user.", ErrorCodes.TechnicalError));
          follower.Following.Remove(userToUnfollow);
          await _repository.UpdateAsync(follower, cancellationToken);
          return ServiceResponse.ForSuccess();
     }

     public async Task<ServiceResponse> FollowTag(Guid tagId, UserDTO requestingUser, CancellationToken cancellationToken = default)
     {
         var tag = await _repository.GetAsync(new TagSpec(tagId), cancellationToken);
         if (tag == null) return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Tag not found.", ErrorCodes.TechnicalError));

         var alreadyFollowing = (await _repository.GetAsync(new UserFollowedTagSpec(requestingUser.Id, tagId), cancellationToken)) != null;
         if (alreadyFollowing) return ServiceResponse.ForSuccess();

         await _repository.AddAsync(new UserFollowedTag { UserId = requestingUser.Id, TagId = tagId }, cancellationToken);
         return ServiceResponse.ForSuccess();
     }

     public async Task<ServiceResponse> UnfollowTag(Guid tagId, UserDTO requestingUser, CancellationToken cancellationToken = default)
     {
          var followRecord = await _repository.GetAsync(new UserFollowedTagSpec(requestingUser.Id, tagId), cancellationToken);
          if (followRecord == null) return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.BadRequest, "Not following this tag.", ErrorCodes.TechnicalError));

          await _repository.DeleteAsync<UserFollowedTag>(followRecord.Id, cancellationToken);
          return ServiceResponse.ForSuccess();
     }

     public async Task<ServiceResponse> SaveReference(Guid referenceId, UserDTO requestingUser, CancellationToken cancellationToken = default)
     {
          var reference = await _repository.GetAsync(new ReferenceSpec(referenceId), cancellationToken);
          if (reference == null) return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Reference not found.", ErrorCodes.TechnicalError));

          var alreadySaved = (await _repository.GetAsync(new SavedReferenceSpec(requestingUser.Id, referenceId), cancellationToken)) != null;
          if (alreadySaved) return ServiceResponse.ForSuccess();

          await _repository.AddAsync(new SavedReference { UserId = requestingUser.Id, ReferenceId = referenceId }, cancellationToken);
          return ServiceResponse.ForSuccess();
     }

     public async Task<ServiceResponse> UnsaveReference(Guid referenceId, UserDTO requestingUser, CancellationToken cancellationToken = default)
     {
          var savedRecord = await _repository.GetAsync(new SavedReferenceSpec(requestingUser.Id, referenceId), cancellationToken);
          if (savedRecord == null) return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.BadRequest, "Reference not saved.", ErrorCodes.TechnicalError));

          await _repository.DeleteAsync<SavedReference>(savedRecord.Id, cancellationToken);
          return ServiceResponse.ForSuccess();
     }

    // Get relationship lists: Use new ServiceResponse factory
    public async Task<ServiceResponse<List<UserSimpleDTO>>> GetFollowing(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetAsync(new UserSpec(userId).IncludeFollows(), cancellationToken);
        if (user == null) return ServiceResponse.FromError<List<UserSimpleDTO>>(new ErrorMessage(HttpStatusCode.NotFound, "User not found.", ErrorCodes.TechnicalError));
        var dtos = user.Following.Select(f => new UserSimpleDTO { Id = f.Id, Name = f.Name, Email = f.Email }).ToList();
        return ServiceResponse.ForSuccess(dtos);
    }

    public async Task<ServiceResponse<List<UserSimpleDTO>>> GetFollowers(Guid userId, CancellationToken cancellationToken = default)
    {
         var user = await _repository.GetAsync(new UserSpec(userId).IncludeFollowers(), cancellationToken);
         if (user == null) return ServiceResponse.FromError<List<UserSimpleDTO>>(new ErrorMessage(HttpStatusCode.NotFound, "User not found.", ErrorCodes.TechnicalError));
         var dtos = user.Followers.Select(f => new UserSimpleDTO { Id = f.Id, Name = f.Name, Email = f.Email }).ToList();
         return ServiceResponse.ForSuccess(dtos);
    }

     public async Task<ServiceResponse<List<TagDTO>>> GetFollowedTags(Guid userId, CancellationToken cancellationToken = default)
     {
         var spec = new UserFollowedTagsSpec(userId);
         var followedTags = await _repository.ListAsync(spec, cancellationToken);
         var dtos = followedTags.Where(uft => uft.Tag != null).Select(uft => new TagDTO { Id = uft.TagId, Name = uft.Tag.Name }).ToList();
         return ServiceResponse.ForSuccess(dtos);
     }

     public async Task<ServiceResponse<List<ReferenceSimpleDTO>>> GetSavedReferences(Guid userId, CancellationToken cancellationToken = default)
     {
         var spec = new SavedReferencesSpec(userId);
         var savedReferences = await _repository.ListAsync(spec, cancellationToken);
         var dtos = savedReferences.Where(sr => sr.Reference != null).Select(sr => new ReferenceSimpleDTO
         {
             Id = sr.ReferenceId, Title = sr.Reference.Title, ImagePath = sr.Reference.ImagePath,
             User = sr.Reference.User == null ? null : new UserSimpleDTO { Id = sr.Reference.UserId, Name = sr.Reference.User.Name, Email = sr.Reference.User.Email }
         }).Where(dto => dto.User != null).ToList();
         return ServiceResponse.ForSuccess(dtos);
     }
}