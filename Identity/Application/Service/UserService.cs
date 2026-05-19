using Identity.Application.Service.Interface;
using Identity.Database.Entity;
using Identity.Database.Repository.Interface;

namespace Identity.Application.Service;

public class UserService(
    IUserRepository userRepository
) : IUserService
{
    public async Task<Guid> CreateUser(CreateUserDto userDto, CancellationToken ct = default) {
        var user = new UserEntity
        {
            Username = userDto.Username,
            Email = userDto.Email
        };

        var userId = await userRepository.CreateUserAsync(user, ct);

        return userId;
    }
}