using Identity.Database.Entity;

namespace Identity.Application.Service.Interface;

/// <summary>
/// Handles token related tasks such as token issuance and validation.
/// </summary>
public interface ITokenService
{
    string CreateToken(UserEntity user, IEnumerable<string>? roles = null);
}