using Identity.Application.Service;
using Identity.Database.Context;
using Identity.Database.Entity;
using Identity.Database.Repository.Interface;

namespace Identity.Database.Repository;

#pragma warning disable CS9107
public class UserRepository(
    AppDbContext db
) : EntityRepository<UserEntity>(db), IUserRepository
{
    public async Task<Guid> CreateUser(UserEntity user, CancellationToken ct = default)
    {
        await db.Users.AddAsync(user, ct);
        
        await db.SaveChangesAsync(ct);

        return user.Id;
    }
}