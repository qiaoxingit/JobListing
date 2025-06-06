using AuthService.Providers;
using AuthService.Repositories;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Contracts.AuthService;
using SharedLib.Extensions;
using System.Composition;

namespace AuthService.Controllers;

/// <summary>
/// Controller responsible for authentication related endpoints
/// </summary>
[ApiController]
[Route("[controller]")]
[method: ImportingConstructor]
public class AuthController(UserRepository userRepository, IJwtProvider jwtProvider) : ControllerBase
{
    /// <summary>
    /// Authenticates a user with the username and password
    /// </summary>
    /// <param name="authenticationRequest">The request containing the username and password</param>
    /// <returns><see cref="AuthenticationResponse"/> indicating if authentication was successful</returns>
    [HttpPost("Authenticate")]
    public async ValueTask<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequest authenticationRequest)
    {
        if (authenticationRequest is null || authenticationRequest.Username.IsNullOrEmpty() || authenticationRequest.Password.IsNullOrEmpty())
        {
            return BadRequest("No username or password specified!");
        }

        var user = await userRepository.GetUserAsync(authenticationRequest.Username, authenticationRequest.Password);

        if (user is null)
        {
            return NotFound("No user found by username and password provided.");
        }

        var authToken = jwtProvider.GenerateToken(user.Id);
        Response.Headers["Authorization"] = $"{authToken}";

        var response = new AuthenticationResponse
        {
            IsAuthenticated = true,
            UserId = user.Id,
            UserName = user.Username,
        };

        return Ok(response);
    }
}
