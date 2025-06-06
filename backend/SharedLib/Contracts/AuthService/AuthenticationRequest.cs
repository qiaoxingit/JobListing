namespace SharedLib.Contracts.AuthService;

/// <summary>
/// Represents the payload sent by the client to authenticate a user
/// </summary>
public class AuthenticationRequest
{
    /// <summary>
    /// Gets or sets the username of the user
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Gets or sets the password of the user
    /// </summary>
    public required string Password { get; set; }
}
