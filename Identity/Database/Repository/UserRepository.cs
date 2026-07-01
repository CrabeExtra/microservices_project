
using Identity.Database.Context;
using Identity.Database.Entity;
using Identity.Database.Repository.Interface;
using Identity.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Database.Repository;

#pragma warning disable CS9107
public class UserRepository(
    AppDbContext db
) : EntityRepository<UserEntity>(db), IUserRepository
{
    //I could use a mapper instead of these but it's the same same efficiency, mapper just adds extra work at this point.

    /// <summary>
    /// Donverts DB entity user to domain user. 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private static User? ToUser(UserEntity? user)
    {
        if(user == null) 
            return null;

        return new User
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            EmailVerified = user.EmailVerified,
            PasswordSet = user.PasswordSet,
            Roles = user.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? []
        };
    }

    /// <summary>
    /// converts domain user to DB entity user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    private static UserEntity ToDbUser(User? user, string password)
    {
        if(user == null) 
            throw new InvalidDataException("User found null when creating database entity.");

        return new UserEntity
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            PasswordHash = password,
            EmailVerified = user.EmailVerified,
            PasswordSet = user.PasswordSet,
        };
    }

    public async Task<Guid> CreateUser(User user, string password, CancellationToken ct)
    {
        await db.Users.AddAsync(ToDbUser(user, password), ct);
        
        await db.SaveChangesAsync(ct);

        return user.Id;
    }

    public async Task<User?> GetById(Guid id, CancellationToken ct) => ToUser(await GetEntityById(id, ct));

    public async Task<User?> GetByEmail(string email, CancellationToken ct) => ToUser(await db.Set<UserEntity>().FirstOrDefaultAsync(u => u.Email == email, ct));

    public async Task<User?> GetByName(string name, CancellationToken ct) => ToUser(await db.Set<UserEntity>().FirstOrDefaultAsync(u => u.Username == name, ct));

    public async Task<ICollection<string>> GetRoles(Guid id, CancellationToken ct) {
        var user = await db.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == id, ct);

        if(user == null)
            return [];
        
        return [.. user.UserRoles?.Select(ur => ur.Role.Name) ?? []];
    }

    public async Task<string?> GetPasswordHash(Guid id, CancellationToken ct) => (await db.Users.FindAsync(id, ct))?.PasswordHash;
}