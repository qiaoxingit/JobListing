namespace SharedLib.Http;

/// <summary>
/// Enumerates each microservice name
/// </summary>
public enum ServiceName
{
    /// <summary>
    /// The name of <see cref="AuthService"/>
    /// </summary>
    AuthService = 0,

    /// <summary>
    /// The name of <see cref="UserService"/>
    /// </summary>
    UserService = 1,

    /// <summary>
    /// The name of <see cref="JobService"/>
    /// </summary>
    JobService = 2,
}
