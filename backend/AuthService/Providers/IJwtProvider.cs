namespace AuthService.Providers;

/// <summary>
/// Provides functionality to generate authentication token
/// </summary>
public interface IJwtProvider
{
    /// <summary>
    /// Generates a token for the specified user
    /// </summary>
    /// <param name="userId">The Id of the user</param>
    /// <returns>A signed token as a string</returns>
    public string GenerateToken(Guid userId);
}
