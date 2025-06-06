using Microsoft.IdentityModel.Tokens;
using SharedLib.Bootstrap;
using System.Composition;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Providers;

/// <summary>
/// Provides implementation for generating authentication tokens
/// </summary>
[Export(typeof(IJwtProvider))]
[Shared]
[method: ImportingConstructor]
public class JwtProvider(AppSettings appSettings) : IJwtProvider
{
    /// <inheritdoc />
    public string GenerateToken(Guid userId)
    {
        var secretKey = appSettings.JwtSettings.Secret;
        var expirationDays = appSettings.JwtSettings.ExpiryDays;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, $"{userId:D}"),
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(expirationDays),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
