﻿namespace SharedLib.Contracts.UserService;

/// <summary>
/// Enumerates user roles in the system
/// </summary>
public enum Role
{
    /// <summary>
    /// A user who can view the job list
    /// </summary>
    Viewer = 0,

    /// <summary>
    /// A user who can post the job
    /// </summary>
    Poster = 1,
}
