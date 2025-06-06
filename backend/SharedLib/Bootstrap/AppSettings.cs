using System.Composition;

namespace SharedLib.Bootstrap;

/// <summary>
/// Represents application wide configuration settings
/// </summary>
[Export]
[Shared]
public class AppSettings
{
    /// <summary>
    /// Gets or sets the current environment (e.g., local, qa, production)
    /// </summary>
    public required string Environment { get; set; }

    /// <summary>
    /// Gets or sets the JWT settings
    /// </summary>
    public required JwtSettings JwtSettings { get; set; }
}

/// <summary>
/// Configuration settings related to JWT
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Gets or sets the secret key used for signing JWT tokens
    /// </summary>
    public required string Secret { get; set; }

    /// <summary>
    /// Gets or sets the number of days after which the token expires
    /// </summary>
    public int ExpiryDays { get; set; }
}