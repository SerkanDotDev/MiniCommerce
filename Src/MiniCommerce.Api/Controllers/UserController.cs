using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCommerce.Api.Common;
using MiniCommerce.Api.Features.Users.Commands;
using MiniCommerce.Api.Features.Users.Queries;
using MiniCommerce.Api.Models.User;

namespace MiniCommerce.Api.Controllers;

/// <summary>
/// Provides endpoints for user management operations
/// </summary>
[ApiController]
[Route("api/user")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Updates user profile
    /// </summary>
    /// <param name="request">User profile update information</param>
    /// <returns>Updated user profile details</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PUT /api/user/update
    ///     {
    ///        "firstName": "John",
    ///        "lastName": "Doe",
    ///        "email": "john.doe@example.com",
    ///        "profilePicture": [binary file]
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">User profile successfully updated</response>
    [Authorize]
    [HttpPut("update")]
    [ProducesResponseType(typeof(GetUserProfileResponse), StatusCodes.Status200OK)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Update([FromForm] UpdateUserRequest request)
    {
        var result = await _mediator.Send(new UpdateUserCommand(User.GetUserId(), request.FirstName, request.LastName, request.Email, request.ProfilePicture));
        return Ok(result);
    }

    /// <summary>
    /// Gets the current user's profile information
    /// </summary>
    /// <returns>User profile information</returns>
    /// <remarks>
    /// Sample response:
    /// 
    ///     {
    ///        "id": 1,
    ///        "firstName": "John",
    ///        "lastName": "Doe",
    ///        "email": "john.doe@example.com",
    ///        "profilePictureUrl": "https://example.com/profiles/john.jpg"
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">User profile successfully retrieved</response>
    [Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(typeof(GetUserProfileResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetUserProfileResponse>> GetProfile()
    {
        var response = await _mediator.Send(new GetUserProfileQuery(User.GetUserId()));
        if (response == null)
            return NotFound();

        return Ok(response);
    }
   
}
