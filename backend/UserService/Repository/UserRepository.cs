using Microsoft.EntityFrameworkCore;
using SharedLib.Contracts.UserService;
using SharedLib.Cryptography;
using SharedLib.Extensions;
using System.Composition;
using UserService.Repository.Database;

namespace UserService.Repository;

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
    /// <param name="token">A token to monitor for cancellation requests</param>
    /// <returns>
    /// A <see cref="User"/> if found; otherwise, <c>null</c>
    /// </returns>
    public async ValueTask<User?> GetAsync(string username, string password, CancellationToken token)
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
        .ToListAsync(token);

        if (users.IsNullOrEmpty() || users.Count > 1)
        {
            return null;
        }

        users[0].Password = null;

        return users[0];
    }
}
