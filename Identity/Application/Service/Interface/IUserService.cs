namespace Identity.Application.Service.Interface;

public interface IUserService
{
    Task<Guid> CreateUser(CreateUserDto userDto, CancellationToken ct = default);
}