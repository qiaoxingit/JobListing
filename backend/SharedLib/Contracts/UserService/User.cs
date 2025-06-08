using System;

namespace SharedLib.Contracts.UserService;

/// <summary>
/// Represents a user in the system
/// </summary>
public class User : Entity
{
    /// <summary>
    /// Gets or sets the first name of the user
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the user
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the email of the user
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the username
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the password
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the role assigned to the user
    /// </summary>
    public Role? Role { get; set; }
}
