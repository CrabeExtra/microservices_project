using Identity.Application.DTOs;

namespace Identity.Application.Service.Interface;

/// <summary>
/// Service for handling auth tasks such as signing up and logging in.
/// </summary>
public interface IAuthService
{
    Task<Guid> SignUp(CreateUserDto dto, CancellationToken ct);

    Task<LoginResponseDto> Login(LoginDto dto, CancellationToken ct);
}