using Identity.Application.DTOs;
using Identity.Application.Service.Interface;
using Identity.Domain.Entity;
using Identity.Database.Repository.Interface;
using Identity.Application.Exceptions;
using Identity.Messaging.Publish.Interface;
using Identity.Messaging.Contract;
namespace Identity.Application.Service;
public class AuthService(
    IUserRepository userRepository,
    IHashService hashService,
    ITokenService tokenService,
    IUserPublish userPublish
) : IAuthService {
    
    public async Task<Guid> SignUp(CreateUserDto dto, CancellationToken ct)
    {
        // 1. Check if user exists
        var existing = await userRepository.GetByEmail(dto.Email, ct);

        if (existing != null)
            throw new ServiceException("User already exists");

        // 2. Create user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email
        };
        
        var password = hashService.Hash(dto.Password);

        // 3. Save
        await userRepository.CreateUser(user, password, ct);

        // 4. publish user created event
        await userPublish.PublishUserCreatedAsync(new CreateUserEventData
        {
            CreatedAt = DateTime.UtcNow,
            OldData = null,
            NewData = new { user.Id, user.Username, user.Email }.ToString(),
            MicroserviceName = "Identity",
            EntityName = "User",
            Action = "Created",
            EventType = "user.created",
            ReferenceId = user.Id
        }, ct);

        return user.Id;
    }

    public async Task<LoginResponseDto> Login(LoginDto dto, CancellationToken ct)
    {
        // 1. Find user
        var user = (await userRepository.GetByEmail(dto.Email, ct)) 
            ?? (await userRepository.GetByName(dto.Username, ct))
            ?? throw new ServiceException("Specified user does not exist.");

        var passwordHash = await userRepository.GetPasswordHash(user.Id, ct) 
            ?? throw new ServiceException($"Password hash found null for existing user with ID {user.Id}. Username: {user.Username}. Invalid scenario.");

        // 2. Verify password
        var valid = hashService.Verify(dto.Password, passwordHash);

        if (!valid)
            throw new ServiceException("Invalid credentials");

        // 4. Generate JWT
        var token = tokenService.CreateToken(user, user.Roles);

        return new LoginResponseDto
        {
            Token = token,
            Email = user.Email,
            Roles = user.Roles
        };
    }
}