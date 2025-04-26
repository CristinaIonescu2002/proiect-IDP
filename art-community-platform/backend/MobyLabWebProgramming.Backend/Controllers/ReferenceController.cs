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
public class ReferenceController : AuthorizedController
{
    private readonly IReferenceService _referenceService;

    public ReferenceController(IUserService userService, IReferenceService referenceService) : base(userService)
    {
        _referenceService = referenceService;
    }

    [AllowAnonymous] // Or AllowAnonymous if references are public
    [HttpGet("{id:guid}")] // Route: /api/References/GetById/{id}
    public async Task<ActionResult<RequestResponse<ReferenceDTO>>> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        return FromServiceResponse(await _referenceService.GetReference(id, cancellationToken));
    }

    [AllowAnonymous] // Or AllowAnonymous if reference lists are public
    [HttpGet] // Route: /api/References/GetList?search=...&userId=...&tagIds=...
    public async Task<ActionResult<RequestResponse<List<ReferenceDTO>>>> GetList([FromQuery] string? search = null, [FromQuery] Guid? userId = null, [FromQuery] List<Guid>? tagIds = null, CancellationToken cancellationToken = default)
    {
        // Note: Returning full ReferenceDTO list might be heavy.
        return FromServiceResponse(await _referenceService.GetReferences(search, userId, tagIds, cancellationToken));
    }

    [AllowAnonymous] // Or AllowAnonymous
    [HttpGet("{id:guid}")] // Route: /api/References/GetArtworksUsingReference/{id}
    public async Task<ActionResult<RequestResponse<List<ArtworkSimpleDTO>>>> GetArtworksUsingReference([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        return FromServiceResponse(await _referenceService.GetArtworksUsingReference(id, cancellationToken));
    }

    [Authorize] // Must be logged in to add a reference
    [HttpPost] // Route: /api/References/Add
    public async Task<ActionResult<RequestResponse>> Add([FromBody] ReferenceAddDTO referenceDto, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await _referenceService.AddReference(referenceDto, currentUser.Result, cancellationToken)) :
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize] // Must be logged in to update (service handles owner/admin check)
    [HttpPut] // Route: /api/References/Update
    public async Task<ActionResult<RequestResponse>> Update([FromBody] ReferenceUpdateDTO referenceDto, CancellationToken cancellationToken = default)
    {
         var currentUser = await GetCurrentUser();
         return currentUser.Result != null ?
             FromServiceResponse(await _referenceService.UpdateReference(referenceDto, currentUser.Result, cancellationToken)) :
             ErrorMessageResult(currentUser.Error);
    }

    [Authorize] // Must be logged in to delete (service handles owner/admin check)
    [HttpDelete("{id:guid}")] // Route: /api/References/Delete/{id}
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await _referenceService.DeleteReference(id, currentUser.Result, cancellationToken)) :
            ErrorMessageResult(currentUser.Error);
    }
}