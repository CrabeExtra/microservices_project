using Identity.Application.DTOs;
using Identity.Domain.Entity;

namespace Identity.Application.Service.Interface;

/// <summary>
/// Service for anything user related.
/// </summary>
public interface IUserService
{   
    /// <summary>
    /// Gets user by ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<User?> GetUser(Guid id, CancellationToken ct);
}