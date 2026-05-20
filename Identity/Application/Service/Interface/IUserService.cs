namespace Identity.Application.Service.Interface;

/// <summary>
/// Service for anything user related.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Creates a new user with the given information.
    /// </summary>
    /// <param name="userDto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<Guid> CreateUser(CreateUserDto userDto, CancellationToken ct = default);
}