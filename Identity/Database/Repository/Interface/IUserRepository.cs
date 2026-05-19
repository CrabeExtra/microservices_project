using Identity.Database.Entity;

namespace Identity.Database.Repository.Interface;

public interface IUserRepository
{
    Task<Guid> CreateUserAsync(UserEntity user, CancellationToken ct = default);
}