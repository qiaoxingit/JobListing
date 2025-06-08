using Microsoft.IdentityModel.Tokens;
using SharedLib.Bootstrap;
using SharedLib.Contracts.UserService;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SharedLib.Cryptography;

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
    public string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSettings.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var expirationDate = DateTime.UtcNow.AddDays(_appSettings.JwtSettings.ExpiryDays);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id!.Value.ToString()),
            new Claim("Role", $"{user.Role}"),
            new Claim("ExpirationDate", $"{expirationDate:yyyyMMddTHHmmss}"),
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expirationDate,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <inheritdoc />
    public List<Claim> GetClaims(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_appSettings.JwtSettings.Secret);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return [.. principal.Claims];
        }
        catch
        {
            return [];
        }
    }
}
