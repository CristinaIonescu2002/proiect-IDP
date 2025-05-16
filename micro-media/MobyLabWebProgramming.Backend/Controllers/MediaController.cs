using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;
// Corrected namespace for interfaces:
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Core.Errors;

namespace MobyLabWebProgramming.API.Controllers; // Your API Controllers namespace

[ApiController]
[Route("api/[controller]/[action]")]
public class MediaController : AuthorizedController
{
    private readonly IMediumService _mediumService;

    public MediaController(IUserService userService, IMediumService mediumService) : base(userService)
    {
        _mediumService = mediumService;
    }

    [AllowAnonymous] // Or AllowAnonymous if media info is public
    [HttpGet("{id:guid}")] // Route: /api/Media/GetById/{id}
    public async Task<ActionResult<RequestResponse<MediumDTO>>> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        return FromServiceResponse(await _mediumService.GetMedium(id, cancellationToken));
    }

    [AllowAnonymous] // Or AllowAnonymous if media list is public
    [HttpGet] // Route: /api/Media/GetList?search=...
    public async Task<ActionResult<RequestResponse<List<MediumDTO>>>> GetList([FromQuery] string? search = null, CancellationToken cancellationToken = default)
    {
        return FromServiceResponse(await _mediumService.GetMedia(search, cancellationToken));
    }

    [Authorize(Roles = "Admin")] // Example: Only Admins can manage Media
    [HttpPost] // Route: /api/Media/Add
    public async Task<ActionResult<RequestResponse>> Add([FromBody] MediumAddDTO mediumDto, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await _mediumService.AddMedium(mediumDto, currentUser.Result, cancellationToken)) :
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut] // Route: /api/Media/Update
    public async Task<ActionResult<RequestResponse>> Update([FromBody] MediumUpdateDTO mediumDto, CancellationToken cancellationToken = default)
    {
         var currentUser = await GetCurrentUser();
         return currentUser.Result != null ?
             FromServiceResponse(await _mediumService.UpdateMedium(mediumDto, currentUser.Result, cancellationToken)) :
             ErrorMessageResult(currentUser.Error);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")] // Route: /api/Media/Delete/{id}
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await _mediumService.DeleteMedium(id, currentUser.Result, cancellationToken)) :
            ErrorMessageResult(currentUser.Error);
    }
}