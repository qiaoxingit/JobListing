using JobService.Repository;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Extensions;

namespace JobService.Controllers;

/// <summary>
/// Controller responsible for Job operations
/// </summary>
[ApiController]
[Route("[controller]")]
public class JobController(JobRepository jobRepository) : ControllerBase
{
    [HttpGet("GetById")]
    public async ValueTask<IActionResult> GetByIdAsync([FromQuery] Guid id, [FromRoute] CancellationToken token)
    {
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
    public async ValueTask<IActionResult> GetAllJobs([FromQuery] int skip = 0, [FromQuery] int take = 10, [FromRoute] CancellationToken token)
    {
        return Ok(await jobRepository.GetPaged(skip, take, token));
    }

    [HttpGet("GetUserInteredJobs")]
    public async ValueTask<IActionResult> GetUserInteredJobsAsync([FromQuery] Guid userId, [FromQuery] int skip = 0, [FromQuery] int take = 10, [FromRoute] CancellationToken token)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("No user id provided.");
        }

        var jobs = await jobRepository.GetUserInteredJobsAsync(userId, skip, take, token);

        if (jobs.IsNullOrEmpty())
        {
            return NotFound("No jobs found.");
        }

        return Ok(jobs);
    }

    [HttpGet("GetUserPostedJobs")]
    public async ValueTask<IActionResult> GetUserPostedJobsAsync([FromQuery] Guid userId, [FromQuery] int skip = 0, [FromQuery] int take = 10, [FromRoute] CancellationToken token)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("No user id provided.");
        }

        var jobs = await jobRepository.GetUserPostedJobsAsync(userId, skip, take, token);

        if (jobs.IsNullOrEmpty())
        {
            return NotFound("No jobs found.");
        }

        return Ok(jobs);
    }
}
