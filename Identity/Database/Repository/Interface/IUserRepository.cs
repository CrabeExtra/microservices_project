using Identity.Application.Service.Interface;
using Identity.Database.Entity;

namespace Identity.Database.Repository.Interface;

/// <summary>
/// Repository for anything user related.
/// </summary>
public interface IUserRepository : IEntityRepository<UserEntity>
{
}