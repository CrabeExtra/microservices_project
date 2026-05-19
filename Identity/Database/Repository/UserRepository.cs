using Identity.Database.Context;
using Identity.Database.Entity;
using Identity.Database.Repository.Interface;

namespace Identity.Database.Repository;

public class UserRepository(
    AppDbContext db
) : IUserRepository
{
    public async Task<Guid> CreateUserAsync(UserEntity user, CancellationToken ct = default)
    {
        await db.Users.AddAsync(user, ct);
        
        await db.SaveChangesAsync(ct);

        return user.Id;
    }
}