using System;

namespace SharedLib.Contracts.JobService;

/// <summary>
/// Represents a job posted in the system
/// </summary>
public class Job : Entity
{
    /// <summary>
    /// Gets or sets the title of the job
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the detailed description of the job
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the job was posted
    /// </summary>
    public DateTime? PostedDate { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the job posting expires
    /// </summary>
    public DateTime? ExpireDate { get; set; }

    /// <summary>
    /// Gets or sets the Id of the user who posted the job
    /// </summary>
    public Guid? PostedByUser { get; set; }
}
