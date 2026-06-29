
using Identity.Domain.Entity;

namespace Identity.Application.Service.Interface;

/// <summary>
/// Handles token related tasks such as token issuance and validation.
/// </summary>
public interface ITokenService
{
    string CreateToken(User user, IEnumerable<string>? roles = null);
}