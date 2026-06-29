using Identity.Application.Service.Interface;
using Identity.Database.Repository.Interface;
using Identity.Domain.Entity;

namespace Identity.Application.Service;

public class UserService(
    IUserRepository userRepository,
    IHashService hashService
) : IUserService
{

    public async Task<User?> GetUser(Guid id, CancellationToken ct) 
        => await userRepository.GetById(id, ct);
   
}