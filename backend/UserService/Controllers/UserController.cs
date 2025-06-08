using Microsoft.AspNetCore.Mvc;
using SharedLib.Contracts.AuthService;
using SharedLib.Contracts.UserService;
using SharedLib.Extensions;
using UserService.Repository;

namespace UserService.Controllers;

/// <summary>
/// Controller responsible for User CRUD
/// </summary>
[ApiController]
[Route("[controller]")]
public class UserController(UserRepository userRepository) : ControllerBase
{
    /// <summary>
    /// Find a <see cref="User"/> by the username
    /// </summary>
    /// <param name="authenticationRequest">The username and password to find the <see cref="User"/></param>
    /// <param name="token">A token to monitor for cancellation requests</param>
    /// <returns>A <see cref="User"/> if found; otherwise <c>null</c></returns>
    [HttpPost("GetUser")]
    public async ValueTask<IActionResult> GetAsync([FromBody] AuthenticationRequest authenticationRequest, [FromRoute] CancellationToken token)
    {
        if (authenticationRequest.Username.IsNullOrEmpty() || authenticationRequest.Password.IsNullOrEmpty())
        {
            return BadRequest("No username or password specified!");
        }

        var user = await userRepository.GetAsync(authenticationRequest.Username, authenticationRequest.Password, token);

        if (user is null)
        {
            return NotFound("No user found by username and password provided.");
        }

        return Ok(user);
    }
}
