using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCommerce.Api.Features.Auth.Commands;
using MiniCommerce.Api.Models.Authentication;

namespace MiniCommerce.Api.Controllers;

/// <summary>
/// Provides endpoints for authentication and authorization operations
/// </summary>
[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="request">User registration information</param>
    /// <returns>JWT token and user information</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/auth/register
    ///     {
    ///        "email": "user@example.com",
    ///        "password": "StrongP@ssw0rd",
    ///        "firstName": "John",
    ///        "lastName": "Doe",
    ///        "profilePhoto": [binary file]
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">User successfully registered</response>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<AuthResponse>> Register([FromForm] RegisterRequest request)
    {
        var command = new RegisterCommand(request.Email, request.Password, request.FirstName, request.LastName, request.ProfilePhoto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Authenticates a user
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>JWT token and user information</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/auth/login
    ///     {
    ///        "email": "user@example.com",
    ///        "password": "StrongP@ssw0rd"
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Login successful</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await _mediator.Send(new LoginCommand(request.Email, request.Password));
        return Ok(result);
    }

    /// <summary>
    /// Refreshes the access token
    /// </summary>
    /// <param name="request">Token refresh information</param>
    /// <returns>New JWT token</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/auth/refresh-token
    ///     {
    ///        "accessToken": "expired.jwt.token",
    ///        "refreshToken": "valid.refresh.token"
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Token successfully refreshed</response>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _mediator.Send(new RefreshTokenCommand(request.AccessToken, request.RefreshToken));
        return Ok(result);
    }
}
