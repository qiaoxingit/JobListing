using Microsoft.EntityFrameworkCore;
using SharedLib.Contracts.UserService;
using SharedLib.Cryptography;
using SharedLib.Database;
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

    /// <summary>
    /// Insert a new user row into USER table if possible
    /// </summary>
    /// <param name="user">The new user to insert into</param>
    /// <param name="token">A token to monitor for cancellation requests</param>
    /// <returns>The number of rows affected</returns>
    public async ValueTask<int> RegisterAsync(User user, CancellationToken token)
    {
        if (user.Username == null || user.Password == null || user.FirstName == null || user.LastName == null || user.Email == null || user.Role == null)
        {
            return -1;
        }
        var encryptedPassword = encryptProvider.Encrypt(user.Password!);

        var rowsAffected = await dbContext.Database.ExecuteSqlInterpolatedAsync
        (
            $@"
            INSERT INTO USER(ID, EMAIL, FIRST_NAME, LAST_NAME, USERNAME, PASSWORD, ROLE)
            VALUES 
            (
                {MySqlGuidConverter.GuidToMySqlBinary(Guid.NewGuid())},
                {user.Email},
                {user.FirstName},
                {user.LastName}, 
                {user.Username}, 
                {encryptedPassword},
                {user.Role}
            )",
             token
        );

        return rowsAffected;
    }

    /// <summary>
    /// Get a list of users who liked on the job
    /// </summary>
    /// <param name="jobId">The id of job which users liked</param>
    /// <param name="token">A token to monitor for cancellation requests</param>
    /// <returns>A list of users who liked on the job</returns>
    public async ValueTask<IReadOnlyList<User>> GetLikedUsersAsync(Guid jobId, CancellationToken token)
    {
        byte[] rawId = MySqlGuidConverter.GuidToMySqlBinary(jobId);

        var users = await dbContext.Users.FromSqlInterpolated
        (
            $@"
            SELECT u.*
              FROM USER u
             INNER JOIN INTERESTEDJOB i
                ON u.ID = i.USER_ID
             WHERE i.JOB_ID = {rawId}"
        )
        .ToListAsync(token);

        users.ForEach(u => u.Password = string.Empty);

        return users;
    }
}
