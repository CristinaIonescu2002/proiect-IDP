using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;
// Corrected namespace for interfaces:
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Core.Errors;

namespace MobyLabWebProgramming.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TagsController : AuthorizedController
{
    private readonly ITagService _tagService;

    public TagsController(IUserService userService, ITagService tagService) : base(userService)
    {
        _tagService = tagService;
    }

    [AllowAnonymous] // Or AllowAnonymous if tags are public
    [HttpGet("{id:guid}")] // Route: /api/Tags/GetById/{id}
    public async Task<ActionResult<RequestResponse<TagDTO>>> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        return FromServiceResponse(await _tagService.GetTag(id, cancellationToken));
    }

    [AllowAnonymous] // Or AllowAnonymous
    [HttpGet("{name}")] // Route: /api/Tags/GetByName/{name}
    public async Task<ActionResult<RequestResponse<TagDTO>>> GetByName(string name, CancellationToken cancellationToken = default)
    {
        return FromServiceResponse(await _tagService.GetTagByName(name, cancellationToken));
    }

    [AllowAnonymous] // Or AllowAnonymous
    [HttpGet] // Route: /api/Tags/GetList?search=...
    public async Task<ActionResult<RequestResponse<List<TagDTO>>>> GetList([FromQuery] string? search = null, CancellationToken cancellationToken = default)
    {
        return FromServiceResponse(await _tagService.GetTags(search, cancellationToken));
    }

    [Authorize(Roles = "Admin")] // Allow any authenticated user to add tags?
    [HttpPost] // Route: /api/Tags/Add
    public async Task<ActionResult<RequestResponse>> Add([FromBody] TagAddDTO tagDto, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await _tagService.AddTag(tagDto, currentUser.Result, cancellationToken)) : // Pass user if service checks role
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize(Roles = "Admin")] // Example: Only Admins can update tags
    [HttpPut] // Route: /api/Tags/Update
    public async Task<ActionResult<RequestResponse>> Update([FromBody] TagUpdateDTO tagDto, CancellationToken cancellationToken = default)
    {
         var currentUser = await GetCurrentUser();
         return currentUser.Result != null ?
             FromServiceResponse(await _tagService.UpdateTag(tagDto, currentUser.Result, cancellationToken)) :
             ErrorMessageResult(currentUser.Error);
    }

    [Authorize(Roles = "Admin")] // Example: Only Admins can delete tags
    [HttpDelete("{id:guid}")] // Route: /api/Tags/Delete/{id}
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await _tagService.DeleteTag(id, currentUser.Result, cancellationToken)) :
            ErrorMessageResult(currentUser.Error);
    }
}