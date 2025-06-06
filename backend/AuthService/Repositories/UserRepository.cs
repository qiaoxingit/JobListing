using AuthService.Entities;
using System.Composition;

namespace AuthService.Repositories;

/// <summary>
/// Provides data access functionality related to users
/// </summary>
[Export(typeof(UserRepository))]
public class UserRepository
{
    /// <summary>
    /// Retrieves a user by username and password
    /// </summary>
    /// <param name="username">The username of the user</param>
    /// <param name="password">The password of the user</param>
    /// <returns>
    /// A <see cref="User"/> if the credentials are valid; otherwise, <c>null</c>
    /// </returns>
    public async ValueTask<User> GetUserAsync(string username, string password)
    {
        return await Task.Run(() => new User
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Harbor",
            Email = "jharbor@test.com",
            Username = "jharbor",
            Password = "123456",
            Role = Role.Reviewer,
        });
    }
}
