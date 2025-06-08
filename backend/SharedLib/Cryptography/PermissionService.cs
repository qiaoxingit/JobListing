using SharedLib.Contracts.JobService;
using SharedLib.Contracts.UserService;
using SharedLib.Extensions;
using System;
using System.Composition;
using System.Globalization;
using System.Linq;

namespace SharedLib.Cryptography;

/// <summary>
/// Provides permission checking functions
/// </summary>
[Export(typeof(IPermissionService))]
public class PermissionService(IJwtProvider jwtProvider) : IPermissionService
{
    /// <inheritdoc />
    public bool DemandPermission(string? token, Role role)
    {
        if (token.IsNullOrEmpty())
        {
            return false;
        }

        var claims = jwtProvider.GetClaims(token!);

        var expirationDateString = claims.FirstOrDefault(c => c.Type.EqualsIgnoreCase("ExpirationDate"))?.Value;

        if (expirationDateString.IsNullOrEmpty())
        {
            return false;
        }

        if (!DateTime.TryParseExact(expirationDateString, "yyyyMMddTHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var expirationDateCliam))
        {
            return false;
        }

        if (expirationDateCliam < DateTime.UtcNow)
        {
            return false;
        }

        var roleString = claims.FirstOrDefault(c => c.Type.EqualsIgnoreCase("Role"))?.Value;

        if (!Enum.TryParse<Role>(roleString, out var roleClaim))
        {
            return false;
        }

        return role == roleClaim;
    }

    /// <inheritdoc />
    public bool DemandPermission(string? token)
    {
        if (token.IsNullOrEmpty())
        {
            return false;
        }

        var claims = jwtProvider.GetClaims(token!);

        var expirationDateString = claims.FirstOrDefault(c => c.Type.EqualsIgnoreCase("ExpirationDate"))?.Value;

        if (expirationDateString.IsNullOrEmpty())
        {
            return false;
        }

        if (!DateTime.TryParseExact(expirationDateString, "yyyyMMddTHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var expirationDateCliam))
        {
            return false;
        }

        return expirationDateCliam >= DateTime.UtcNow;
    }

    /// <inheritdoc />
    public bool DemandPermission(string? token, Job job)
    {
        if (token.IsNullOrEmpty())
        {
            return false;
        }

        var claims = jwtProvider.GetClaims(token!);

        if (!Guid.TryParse(claims.FirstOrDefault(c => c.Type.EqualsIgnoreCase("UserId"))?.Value, out var userId))
        {
            return false;
        }

        return job.PostedByUser.HasValue && userId == job.PostedByUser.Value;
    }
}
