namespace AuthService.Repositories.Entities;

/// <summary>
/// Enumerates user roles in the system
/// </summary>
public enum Role
{
    /// <summary>
    /// A user who can review the job list
    /// </summary>
    Reviewer = 0,

    /// <summary>
    /// A user who can post the job
    /// </summary>
    Poster = 1,
}
