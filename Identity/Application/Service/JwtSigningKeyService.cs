using System.Security.Cryptography;
using Identity.Application.Exceptions;
using Identity.Application.Service.Interface;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Application.Service;

public class JwtSigningKeyService : IJwtSigningKeyService
{
    private readonly SigningCredentials _signingCredentials;

    public JwtSigningKeyService()
    {
       var base64 = Environment.GetEnvironmentVariable("JWT_PRIVATE_KEY_BASE64");

        if(string.IsNullOrWhiteSpace(base64))
        {
            throw new ServiceException("JWT private key not found in environment variables.");
        }

        var keyBytes = Convert.FromBase64String(base64);
        var privateKeyPem = System.Text.Encoding.UTF8.GetString(keyBytes);

        var rsa = RSA.Create();
        rsa.ImportFromPem(privateKeyPem);

        var key = new RsaSecurityKey(rsa);

        _signingCredentials = new SigningCredentials(
            key,
            SecurityAlgorithms.RsaSha256
        );

    }

    public SigningCredentials GetSigningCredentials() => _signingCredentials;
}