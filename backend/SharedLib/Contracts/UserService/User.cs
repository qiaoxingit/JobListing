using System;

namespace SharedLib.Contracts.UserService;

/// <summary>
/// Represents a user in the system
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the Id for the user
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the first name of the user
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the user
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Gets or sets the email of the user
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the username
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Gets or sets the password
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the role assigned to the user
    /// </summary>
    public required Role Role { get; set; }
}
