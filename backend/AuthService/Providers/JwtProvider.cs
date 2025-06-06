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
public class JwtProvider : IJwtProvider
{
    private readonly AppSettings _appSettings;

    public JwtProvider(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    /// <inheritdoc />
    public string GenerateToken(Guid userId)
    {
        var secretKey = _appSettings.JwtSettings.Secret;
        var expirationDays = _appSettings.JwtSettings.ExpiryDays;

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
