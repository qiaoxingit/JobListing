using Microsoft.AspNetCore.Mvc;
using SharedLib.Contracts.AuthService;
using SharedLib.Contracts.UserService;
using SharedLib.Cryptography;
using SharedLib.Extensions;
using SharedLib.Http;

namespace AuthService.Controllers;

/// <summary>
/// Controller responsible for authentication related endpoints
/// </summary>
[ApiController]
[Route("[controller]")]
public class AuthController(IJwtProvider jwtProvider, IHttpClientFactory httpClientFactory) : ControllerBase
{
    /// <summary>
    /// Authenticates a user with the username and password
    /// </summary>
    /// <param name="authenticationRequest">The request containing the username and password</param>
    /// <param name="token">A token to monitor for cancellation requests</param>
    /// <returns><see cref="AuthenticationResponse"/> indicating if authentication was successful</returns>
    [HttpPost("Authenticate")]
    public async ValueTask<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequest authenticationRequest, [FromRoute] CancellationToken token)
    {
        if (authenticationRequest is null || authenticationRequest.Username.IsNullOrEmpty() || authenticationRequest.Password.IsNullOrEmpty())
        {
            return BadRequest("No username or password specified!");
        }

        var userApi = httpClientFactory.CreateClient(nameof(ServiceName.UserService));
        var user = await userApi.PostAsJsonAsync("user/GetUser", authenticationRequest, token)
            .ContinueWith
            (
                async postTask =>
                {
                    var response = await postTask;

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadFromJsonAsync<User?>();
                    }

                    return null;
                }
            )
            .Unwrap();

        if (user is null)
        {
            return NotFound("No user found by username and password provided.");
        }

        var authToken = jwtProvider.GenerateToken(user);
        Response.Headers["Authorization"] = $"{authToken}";

        var response = new AuthenticationResponse
        {
            IsAuthenticated = true,
            UserId = user.Id,
            UserName = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
        };

        return Ok(response);
    }
}
