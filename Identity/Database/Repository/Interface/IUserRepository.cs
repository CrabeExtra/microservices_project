using Identity.Application.Service.Interface;
using Identity.Database.Entity;
using Identity.Domain.Entity;

namespace Identity.Database.Repository.Interface;

/// <summary>
/// Repository for anything user related.
/// </summary>
public interface IUserRepository : IEntityRepository<UserEntity>
{
    /// <summary>
    /// Creates a user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<Guid> CreateUser(User user, string password, CancellationToken ct);
    /// <summary>
    /// Gets a user by their email field.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<User?> GetByEmail(string email, CancellationToken ct);
    /// <summary>
    /// Gets a user by their ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<User?> GetById(Guid id, CancellationToken ct);
    /// <summary>
    /// Gets a user by their name field.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<User?> GetByName(string name, CancellationToken ct);
    /// <summary>
    /// Gets a user's roles by their ID field.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<ICollection<string>> GetRoles(Guid id, CancellationToken ct);
    /// <summary>
    /// Gets the password of the user with provided ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<string?> GetPasswordHash(Guid id, CancellationToken ct);
}