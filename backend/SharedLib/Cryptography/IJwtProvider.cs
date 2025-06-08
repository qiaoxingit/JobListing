using SharedLib.Contracts.UserService;
using System.Collections.Generic;
using System.Security.Claims;

namespace SharedLib.Cryptography;

/// <summary>
/// Provides functionality to generate authentication token
/// </summary>
public interface IJwtProvider
{
    /// <summary>
    /// Generates a token for the specified user
    /// </summary>
    /// <param name="user">The user to generate the token</param>
    /// <returns>A signed token as a string</returns>
    public string GenerateToken(User user);

    /// <summary>
    /// Parses token
    /// </summary>
    /// <param name="token">Token to be parsed</param>
    /// <returns>A list of <see cref="Claim"/> parsed from the token</returns>
    public List<Claim> GetClaims(string token);
}
