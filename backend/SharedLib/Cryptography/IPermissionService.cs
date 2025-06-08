using SharedLib.Contracts.UserService;

namespace SharedLib.Cryptography;

/// <summary>
/// Provides permission checking functions
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Check if token has not expired and has proper role assigned
    /// </summary>
    /// <param name="token">The token to be checked</param>
    /// <param name="role">The role to be compared</param>
    /// <returns><c>true</c> if permission is satisfied; otherwise <c>false</c></returns>
    public bool DemandPermission(string? token, Role role);

    /// <summary>
    /// Check if token has not expired
    /// </summary>
    /// <param name="token">The token to be checked</param>
    /// <returns><c>true</c> if permission is satisfied; otherwise <c>false</c></returns>
    public bool DemandPermission(string? token);
}
