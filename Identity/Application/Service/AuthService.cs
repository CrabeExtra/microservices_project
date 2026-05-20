using Identity.Application.Service.Interface;
using Identity.Database.Entity;
using Identity.Database.Repository.Interface;

namespace Identity.Application.Service;
public class AuthService(
    IUserRepository userRepository,
    IHashService hashService,
    ITokenService tokenService
) : IAuthService {
    /// TODO: 
    // - debug,
    // - implement GetByEmail
    // - add loginDto
    // - Implement AuthController
    // 
    public async Task<Guid> SignUp(CreateUserDto dto, CancellationToken ct)
    {
        // 1. Check if user exists
        var existing = await userRepository.GetByEmail(dto.Email, ct);
        if (existing != null)
            throw new InvalidOperationException("User already exists");

        // 2. Create user
        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = hashService.Hash(dto.Password)
        };

        // 3. Save
        await userRepository.CreateUser(user, ct);

        return user.Id;
    }

    public async Task<string> Login(LoginDto dto, CancellationToken ct)
    {
        // 1. Find user
        var user = await userRepository.GetByEmail(dto.Email, ct);

        if (user == null)
            throw new InvalidOperationException("Invalid credentials");

        // 2. Verify password
        var valid = hashService.Verify(dto.Password, user.PasswordHash);

        if (!valid)
            throw new InvalidOperationException("Invalid credentials");

        // 3. Get roles (from DB)
        var roles = await userRepository.GetRoles(user.Id, ct);

        // 4. Generate JWT
        var token = tokenService.CreateToken(user, roles);

        return token;
    }
}