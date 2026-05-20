using Microsoft.IdentityModel.Tokens;

namespace Identity.Application.Service.Interface;

public interface IJwtSigningKeyService
{
    SigningCredentials GetSigningCredentials();
}
