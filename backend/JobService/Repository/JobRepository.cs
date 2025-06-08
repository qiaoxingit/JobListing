using JobService.Repository.Database;
using Microsoft.EntityFrameworkCore;
using SharedLib.Contracts.JobService;
using SharedLib.Database;
using SharedLib.Extensions;
using System.Composition;

namespace JobService.Repository;

/// <summary>
/// Provides data access functionality related to users
/// </summary>
[Export(typeof(JobRepository))]
public class JobRepository(DatabaseContext dbContext)
{
    private const int recentMonthThreshold = -2;
    private const int expirationMonth = 2;

    /// <summary>
    /// Retrieves a job by ID
    /// </summary>
    /// <param name="jobId">The id of the job</param>
    /// <param name="token">A token to monitor for cancellation requests</param>
    /// <returns>
    /// A <see cref="Job?"/> if found; otherwise, <c>null</c>
    /// </returns>
    public async ValueTask<Job?> GetByIdAsync(Guid jobId, CancellationToken token)
    {
        var cutoffDate = DateTime.UtcNow.AddMonths(recentMonthThreshold);

        byte[] rawId = MySqlGuidConverter.GuidToMySqlBinary(jobId);

        var jobs = await dbContext.Jobs.FromSqlInterpolated
        (
            $"SELECT * FROM JOB WHERE ID = {rawId} AND DATE_POSTED >= {cutoffDate}"
        )
        .ToListAsync(token);

        if (jobs.IsNullOrEmpty() || jobs.Count > 1)
        {
            return null;
        }

        return jobs[0];
    }

    /// <summary>
    /// Retrieves a list of jbos that the user has interest
    /// </summary>
    /// <param name="userId">The user who has interet</param>
    /// <param name="skip">the number of jobs to skip from the result</param>
    /// <param name="take">the number of jobs to return from the result</param>
    /// <param name="token">A token to monitor for cancellation requests</param>
    /// <returns>A list of jobs that the user has interest</returns>
    public async ValueTask<IReadOnlyList<Job>> GetUserInteredJobsAsync(Guid userId, int skip, int take, CancellationToken token)
    {
        var cutoffDate = DateTime.UtcNow.AddMonths(recentMonthThreshold);

        byte[] rawId = MySqlGuidConverter.GuidToMySqlBinary(userId);

        var jobs = await dbContext.Jobs.FromSqlInterpolated
        (
            $@"
            SELECT j.* FROM INTERESTEDJOB i
              LEFT JOIN JOB j
                ON i.JOB_ID = j.ID
             WHERE USER_ID = {rawId} AND DATE_POSTED >= {cutoffDate}
             ORDER BY DATE_POSTED DESC
             LIMIT {take} OFFSET {skip}"
        )
        .ToListAsync(token);

        if (jobs.IsNullOrEmpty())
        {
            return [];
        }

        return jobs;
    }

    /// <summary>
    /// Retrieves a list of jbos that posted by the user
    /// </summary>
    /// <param name="userId">The user who posted the jobs</param>
    /// <param name="skip">the number of jobs to skip from the result</param>
    /// <param name="take">the number of jobs to return from the result</param>
    /// <param name="token">A token to monitor for cancellation requests</param>
    /// <returns>A list of jobs that has been posted by the user</returns>
    public async ValueTask<IReadOnlyList<Job>> GetUserPostedJobsAsync(Guid userId, int skip, int take, CancellationToken token)
    {
        var cutoffDate = DateTime.UtcNow.AddMonths(recentMonthThreshold);

        byte[] rawId = MySqlGuidConverter.GuidToMySqlBinary(userId);

        var jobs = await dbContext.Jobs.FromSqlInterpolated
        (
            $@"
            SELECT * FROM JOB
             WHERE POSTED_BY_USER = {rawId} AND DATE_POSTED >= {cutoffDate}
             ORDER BY DATE_POSTED DESC
             LIMIT {take} OFFSET {skip}"
        )
        .ToListAsync(token);

        if (jobs.IsNullOrEmpty())
        {
            return [];
        }

        return jobs;
    }

    /// <summary>
    /// Get a paged result of all jobs
    /// </summary>
    /// <param name="skip">the number of jobs to skip from the result</param>
    /// <param name="take">the number of jobs to return from the result</param>
    /// <param name="token">A token to monitor for cancellation requests</param>
    /// <returns>A list of jobs</returns>
    public async ValueTask<IReadOnlyList<Job>> GetPaged(int skip, int take, CancellationToken token)
    {
        var cutoffDate = DateTime.UtcNow.AddMonths(recentMonthThreshold);

        var jobs = await dbContext.Jobs.FromSqlInterpolated
        (
            $@"
            SELECT * FROM JOB
             WHERE DATE_POSTED >= {cutoffDate}
             ORDER BY DATE_POSTED DESC
             LIMIT {take} OFFSET {skip}"
        )
        .ToListAsync(token);

        if (jobs.IsNullOrEmpty())
        {
            return [];
        }

        return jobs;
    }

    /// <summary>
    /// Updates job
    /// </summary>
    /// <param name="job">The job to be updated</param>
    /// <param name="token">A token to monitor for cancellation requests</param>
    public async ValueTask<int> UpdateJobAsync(Job job, CancellationToken token)
    {
        byte[] rawId = MySqlGuidConverter.GuidToMySqlBinary(job.Id);

        var rowsAffected = await dbContext.Database.ExecuteSqlInterpolatedAsync
        (
            $@"
            UPDATE JOB
               SET TITLE = {job.Title},
                   DESCRIPTION = {job.Description}
             WHERE ID = {rawId}",
             token
        );

        return rowsAffected;
    }

    /// <summary>
    /// Creates job
    /// </summary>
    /// <param name="job">The job to be created</param>
    /// <param name="token">A token to monitor for cancellation requests</param>
    public async ValueTask<int> CreateJobAsync(Job job, CancellationToken token)
    {
        var postedDate = DateTime.UtcNow;
        var expiredDate = postedDate.AddMonths(expirationMonth);

        var rowsAffected = await dbContext.Database.ExecuteSqlInterpolatedAsync
        (
            $@"
            INSERT INTO JOB(ID, TITLE, DESCRIPTION, DATE_POSTED, DATE_EXPIRED, POSTED_BY_USER)
            VALUES 
            (
                {MySqlGuidConverter.GuidToMySqlBinary(Guid.NewGuid())},
                {job.Title},
                {job.Description},
                {postedDate}, 
                {expiredDate}, 
                {MySqlGuidConverter.GuidToMySqlBinary(job.PostedByUser)}
            )",
             token
        );

        return rowsAffected;
    }
}
