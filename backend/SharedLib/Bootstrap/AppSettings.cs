using System.Collections.Generic;

namespace SharedLib.Bootstrap;

/// <summary>
/// Represents application wide configuration settings
/// </summary>
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

    /// <summary>
    /// Gets or sets the fixed salt for encryption
    /// </summary>
    public required string FixedSalt { get; set; }

    /// <summary>
    /// Gets or sets the database connection strings
    /// </summary>
    public required DBCongfigration DBCongfigration { get; set; }

    /// <summary>
    /// Gets or sets the list of HttpClients
    /// </summary>
    public IReadOnlyList<HttpClientConfig>? HttpClients { get; set; }
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

/// <summary>
/// Represents the database connection strings used by the application
/// </summary>
public class DBCongfigration
{
    /// <summary>
    /// Gets or sets the MySql database version
    /// </summary>
    public required string MySqlVersion { get; set; }

    /// <summary>
    /// Gets or sets the main database connection string
    /// </summary>
    public required string DefaultConnection { get; set; }
}

/// <summary>
/// Represents an HttpClient's configurations
/// </summary>
public class HttpClientConfig
{
    /// <summary>
    /// Gets or sets the name of the configuration
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the base URL of each service
    /// </summary>
    public required string ServiceBaseUrl { get; set; }
}