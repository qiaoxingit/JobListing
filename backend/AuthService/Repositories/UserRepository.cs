using AuthService.Providers;
using AuthService.Repositories.Database;
using AuthService.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using SharedLib.Extensions;
using System.Composition;

namespace AuthService.Repositories;

/// <summary>
/// Provides data access functionality related to users
/// </summary>
[Export(typeof(UserRepository))]
public class UserRepository(DatabaseContext dbContext, IEncryptProvider encryptProvider)
{
    /// <summary>
    /// Retrieves a user by username and password
    /// </summary>
    /// <param name="username">The username of the user</param>
    /// <param name="password">The password of the user</param>
    /// <returns>
    /// A <see cref="User"/> if the credentials are valid; otherwise, <c>null</c>
    /// </returns>
    public async ValueTask<User?> GetUserAsync(string username, string password)
    {
        var encryptedPassword = encryptProvider.Encrypt(password);
        var users = await dbContext.Users.FromSqlInterpolated
        (
            $@"
            SELECT *
            FROM USER
            WHERE USERNAME = {username}
              AND PASSWORD = {encryptedPassword}"
        )
        .ToListAsync();

        if (users.IsNullOrEmpty() || users.Count > 1)
        {
            return null;
        }

        return users[0];
    }
}
