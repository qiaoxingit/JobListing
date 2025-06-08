using SharedLib.Contracts.UserService;
using System;

namespace SharedLib.Contracts.AuthService;

/// <summary>
/// Represents the response returned after an authentication attempt
/// </summary>
public class AuthenticationResponse
{
    /// <summary>
    /// Gets or sets a value indicating whether the authentication was successful
    /// </summary>
    public bool IsAuthenticated { get; set; }

    /// <summary>
    /// Gets or sets the Id of the authenticated user
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the username of the authenticated user
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    /// Gets or sets the firstname of the authenticated user
    /// </summary>
    public required string FirstName {  get; set; }

    /// <summary>
    /// Gets or sets the lastname of the authenticated user
    /// </summary>
    public required string LastName { get; set; }

    public required Role Role { get; set; }
}
