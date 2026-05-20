using Identity.Application.Service.Interface;
using Identity.Database.Entity;
using Identity.Database.Repository.Interface;

namespace Identity.Application.Service;

public class UserService(
    IUserRepository userRepository,
    IHashService hashService
) : IUserService
{
    public async Task<Guid> CreateUser(CreateUserDto userDto, CancellationToken ct = default) {
        var user = new UserEntity
        {
            Username = userDto.Username,
            Email = userDto.Email,
            PasswordHash = hashService.Hash(userDto.Password)
        };

        var userId = await userRepository.CreateUser(user, ct);

        return userId;
    }
}