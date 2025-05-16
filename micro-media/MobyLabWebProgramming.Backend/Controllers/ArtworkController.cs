using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects; // For ArtworkDTO, ArtworkAddDTO, ArtworkUpdateDTO etc.
using MobyLabWebProgramming.Core.Responses;         // For RequestResponse<T>
using MobyLabWebProgramming.Infrastructure.Services.Interfaces; // For IArtworkService, IUserService
using MobyLabWebProgramming.Infrastructure.Authorization; // For AuthorizedController base class
using MobyLabWebProgramming.Core.Errors;             // For ErrorMessage

namespace MobyLabWebProgramming.API.Controllers; // Your API Controllers namespace

/// <summary>
/// Controller for managing Artworks.
/// Follows the pattern of inheriting AuthorizedController and using action routing.
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")] // Route: /api/Artworks/[action]
public class ArtworksController : AuthorizedController // Inherit from base
{
    // Service specific to this controller's domain
    private readonly IArtworkService _artworkService;

    // Constructor injects IUserService for the base class and IArtworkService for artwork operations
    public ArtworksController(IUserService userService, IArtworkService artworkService) : base(userService)
    {
        _artworkService = artworkService;
    }

    /// <summary>
    /// Gets details of a specific Artwork by its ID.
    /// </summary>
    [AllowAnonymous] // Or [AllowAnonymous] if artworks are public
    [HttpGet("{id:guid}")] // Route: /api/Artworks/GetById/{id}
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequestResponse<ArtworkDTO>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(RequestResponse))]
    public async Task<ActionResult<RequestResponse<ArtworkDTO>>> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        // Directly call the service method using the helper from the base class
        return FromServiceResponse(await _artworkService.GetArtwork(id, cancellationToken));
    }

    /// <summary>
    /// Gets a list of Artworks, potentially filtered.
    /// </summary>
    [AllowAnonymous] // Or [AllowAnonymous] if artwork lists are public
    [HttpGet] // Route: /api/Artworks/GetList?search=...&userId=...&mediumId=...&tagIds=...
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequestResponse<List<ArtworkDTO>>))]
    public async Task<ActionResult<RequestResponse<List<ArtworkDTO>>>> GetList([FromQuery] string? search = null, [FromQuery] Guid? userId = null, [FromQuery] Guid? mediumId = null, [FromQuery] List<Guid>? tagIds = null, CancellationToken cancellationToken = default)
    {
        // Note: Returning a list of full ArtworkDTO might be inefficient for large datasets or complex objects.
        // Consider using ArtworkSimpleDTO if performance becomes an issue.
        return FromServiceResponse(await _artworkService.GetArtworks(search, userId, mediumId, tagIds, cancellationToken));
    }

    /// <summary>
    /// Adds a new Artwork. Requires authentication.
    /// </summary>
    [Authorize] // User must be logged in to add artwork
    [HttpPost] // Route: /api/Artworks/Add
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RequestResponse))] // Or 200 OK if preferred
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(RequestResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(RequestResponse))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(RequestResponse))]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] ArtworkAddDTO artworkDto, CancellationToken cancellationToken = default)
    {
        // Get the currently logged-in user DTO from the base controller helper
        var currentUser = await GetCurrentUser();
        // Check if user retrieval was successful before proceeding
        return currentUser.Result != null ?
            FromServiceResponse(await _artworkService.AddArtwork(artworkDto, currentUser.Result, cancellationToken)) :
            ErrorMessageResult(currentUser.Error); // Use base helper to handle error if user couldn't be retrieved
        // Note: FromServiceResponse currently returns Ok(response). For proper 201 Created, adjust base helper or controller return:
        // var result = await _artworkService.AddArtwork(artworkDto, currentUser.Result, cancellationToken);
        // return result.IsOk ? StatusCode(StatusCodes.Status201Created, RequestResponse.FromSuccess()) : ErrorMessageResult(result.Error);
    }

    /// <summary>
    /// Updates an existing Artwork. Requires authentication and ownership/permission.
    /// </summary>
    [Authorize] // User must be logged in
    [HttpPut] // Route: /api/Artworks/Update
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequestResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(RequestResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(RequestResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(RequestResponse))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(RequestResponse))]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] ArtworkUpdateDTO artworkDto, CancellationToken cancellationToken = default)
    {
         // Get the currently logged-in user DTO
         var currentUser = await GetCurrentUser();
         // Check if user retrieval was successful before proceeding
         return currentUser.Result != null ?
             FromServiceResponse(await _artworkService.UpdateArtwork(artworkDto, currentUser.Result, cancellationToken)) :
             ErrorMessageResult(currentUser.Error);
    }

    /// <summary>
    /// Deletes an Artwork. Requires authentication and ownership/permission.
    /// </summary>
    [Authorize] // User must be logged in
    [HttpDelete("{id:guid}")] // Route: /api/Artworks/Delete/{id}
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequestResponse))] // Or 204 No Content
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(RequestResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(RequestResponse))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(RequestResponse))]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        // Get the currently logged-in user DTO
        var currentUser = await GetCurrentUser();
        // Check if user retrieval was successful before proceeding
        return currentUser.Result != null ?
            FromServiceResponse(await _artworkService.DeleteArtwork(id, currentUser.Result, cancellationToken)) :
            ErrorMessageResult(currentUser.Error);
    }
}