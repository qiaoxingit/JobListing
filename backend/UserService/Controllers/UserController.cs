using Microsoft.AspNetCore.Mvc;
using SharedLib.Contracts.AuthService;
using SharedLib.Contracts.JobService;
using SharedLib.Contracts.UserService;
using SharedLib.Cryptography;
using SharedLib.Extensions;
using SharedLib.Http;
using System.Net.Http.Headers;
using UserService.Repository;

namespace UserService.Controllers;

/// <summary>
/// Controller responsible for User CRUD
/// </summary>
[ApiController]
[Route("[controller]")]
public class UserController
(
    UserRepository userRepository,
    IPermissionService permissionService,
    IHttpClientFactory httpClientFactory
)
    : ControllerBase
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

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="user">The new user to register</param>
    /// <param name="token">A token to monitor for cancellation requests</param>
    [HttpPost("Register")]
    public async ValueTask<IActionResult> RegisterAsync([FromBody] User user, [FromRoute] CancellationToken token)
    {
        if (user.Username == null || user.Password == null || user.FirstName == null || user.LastName == null || user.Email == null || user.Role == null)
        {
            return BadRequest();
        }
        var rowAffected = await userRepository.RegisterAsync(user, token);

        if (rowAffected != 1)
        {
            return NotFound("The email is already used.");
        }

        return Ok();
    }

    /// <summary>
    /// Get a list of users who liked on the job
    /// </summary>
    /// <param name="authToken">The authorization token to authenticate the request</param>
    /// <param name="jobId">The id of the job to check that which user likes</param>
    /// <param name="token">A token to monitor for cancellation requests</param>
    /// <returns>A list of users who liked on the job</returns>
    [HttpGet("GetLikedUsers")]
    public async ValueTask<IActionResult> GetLikedUsersAsync([FromHeader(Name = "Authorization")] string? authToken, [FromQuery] Guid jobId, [FromRoute] CancellationToken token)
    {
        if (!permissionService.DemandPermission(authToken, Role.Poster))
        {
            return Unauthorized();
        }

        var jobApi = httpClientFactory.CreateClient(nameof(ServiceName.JobService));
        jobApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken!.Replace("Bearer", ""));
        var job = await jobApi.GetFromJsonAsync<Job>($"job/GetById?id={jobId}", token);

        if (job is null)
        {
            return NotFound($"Job/{jobId} is not found.");
        }

        if (!permissionService.DemandPermission(authToken, job))
        {
            return Unauthorized();
        }

        var users = await userRepository.GetLikedUsersAsync(jobId, token);

        return Ok(users ?? []);
    }
}
