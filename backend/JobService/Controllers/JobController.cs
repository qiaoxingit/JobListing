using JobService.Repository;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Contracts.JobService;
using SharedLib.Contracts.UserService;
using SharedLib.Cryptography;
using SharedLib.Extensions;

namespace JobService.Controllers;

/// <summary>
/// Controller responsible for Job operations
/// </summary>
[ApiController]
[Route("[controller]")]
public class JobController(JobRepository jobRepository, IPermissionService permissionService) : ControllerBase
{
    [HttpGet("GetById")]
    public async ValueTask<IActionResult> GetByIdAsync([FromHeader(Name = "Authorization")] string? authToken, [FromQuery] Guid id, [FromRoute] CancellationToken token)
    {
        if (!permissionService.DemandPermission(authToken))
        {
            return Unauthorized();
        }

        if (id == Guid.Empty)
        {
            return BadRequest("No job id provided.");
        }

        var job = await jobRepository.GetByIdAsync(id, token);

        if (job is null)
        {
            return NotFound("No jobs found.");
        }

        return Ok(job);
    }

    [HttpGet("GetPaged")]
    public async ValueTask<IActionResult> GetAllJobs([FromHeader(Name = "Authorization")] string? authToken, [FromRoute] CancellationToken token, [FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        if (!permissionService.DemandPermission(authToken))
        {
            return Unauthorized();
        }

        return Ok(await jobRepository.GetPaged(skip, take, token));
    }

    [HttpGet("GetUserInteredJobs")]
    public async ValueTask<IActionResult> GetUserInteredJobsAsync([FromHeader(Name = "Authorization")] string? authToken, [FromQuery] Guid userId, [FromRoute] CancellationToken token, [FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        if (!permissionService.DemandPermission(authToken, Role.Viewer))
        {
            return Unauthorized();
        }

        if (userId == Guid.Empty)
        {
            return BadRequest("No user id provided.");
        }

        var result = await jobRepository.GetUserInteredJobsAsync(userId, skip, take, token);

        return Ok(result);
    }

    [HttpGet("GetUserPostedJobs")]
    public async ValueTask<IActionResult> GetUserPostedJobsAsync([FromHeader(Name = "Authorization")] string? authToken, [FromQuery] Guid userId, [FromRoute] CancellationToken token, [FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        if (!permissionService.DemandPermission(authToken, Role.Poster))
        {
            return Unauthorized();
        }

        if (userId == Guid.Empty)
        {
            return BadRequest("No user id provided.");
        }

        var result = await jobRepository.GetUserPostedJobsAsync(userId, skip, take, token);

        return Ok(result);
    }

    [HttpGet("UpdateJob")]
    public async ValueTask<IActionResult> UpdateJobAsync([FromHeader(Name = "Authorization")] string? authToken, [FromBody] Job job, [FromRoute] CancellationToken token)
    {
        if (!permissionService.DemandPermission(authToken, Role.Poster))
        {
            return Unauthorized();
        }

        if (job is null)
        {
            return BadRequest("No job is provided.");
        }

        if (job.Id == Guid.Empty)
        {
            return BadRequest("No jobId is provided.");
        }

        if (job.Title.IsNullOrEmpty())
        {
            return BadRequest("No job title is provided.");
        }

        if (job.Description.IsNullOrEmpty())
        {
            return BadRequest("No job description is provided.");
        }

        var oldJob = await jobRepository.GetByIdAsync(job.Id, token);

        if (oldJob is null)
        {
            return NotFound($"Job/{job.Id} doesn't exist.");
        }

        var rowsAffected = await jobRepository.UpdateJobAsync(job, token);

        if (rowsAffected != 1)
        {
            return Conflict($"There is {rowsAffected} rows updated, which is unexpected.");
        }

        return Ok();
    }

    [HttpGet("CreateJob")]
    public async ValueTask<IActionResult> CreateJobAsync([FromHeader(Name = "Authorization")] string? authToken, [FromBody] Job job, [FromRoute] CancellationToken token)
    {
        if (!permissionService.DemandPermission(authToken, Role.Poster))
        {
            return Unauthorized();
        }

        if (job is null)
        {
            return BadRequest("No job is provided.");
        }

        if (job.Title.IsNullOrEmpty())
        {
            return BadRequest("No job title is provided.");
        }

        if (job.Description.IsNullOrEmpty())
        {
            return BadRequest("No job description is provided.");
        }

        if (job.PostedByUser == Guid.Empty)
        {
            return BadRequest("No user ID is provided.");
        }

        var rowsAffected = await jobRepository.CreateJobAsync(job, token);

        if (rowsAffected != 1)
        {
            return Conflict($"There is {rowsAffected} rows created, which is unexpected.");
        }

        return Ok();
    }
}
