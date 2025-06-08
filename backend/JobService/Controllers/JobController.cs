using JobService.Repository;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Contracts.JobService;
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
    public async ValueTask<IActionResult> GetAllJobs([FromRoute] CancellationToken token, [FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return Ok(await jobRepository.GetPaged(skip, take, token));
    }

    [HttpGet("GetUserInteredJobs")]
    public async ValueTask<IActionResult> GetUserInteredJobsAsync([FromQuery] Guid userId, [FromRoute] CancellationToken token, [FromQuery] int skip = 0, [FromQuery] int take = 10)
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
    public async ValueTask<IActionResult> GetUserPostedJobsAsync([FromQuery] Guid userId, [FromRoute] CancellationToken token, [FromQuery] int skip = 0, [FromQuery] int take = 10)
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

    [HttpGet("UpdateJob")]
    public async ValueTask<IActionResult> UpdateJobAsync([FromBody] Job job, [FromRoute] CancellationToken token)
    {
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
}
