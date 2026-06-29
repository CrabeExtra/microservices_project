using Identity.Application.Service.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Identity.Domain.Entity;

namespace Identity.Application.Service;

public class TokenService : ITokenService
{

    private readonly SigningCredentials _creds;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiresHours;

    public TokenService(
        IJwtSigningKeyService keyService,
        IConfiguration configuration
    )
    {
        _creds = keyService.GetSigningCredentials();

        _issuer = configuration.GetValue<string>("Jwt:Issuer")
            ?? throw new InvalidOperationException("Jwt:Issuer missing");

        _audience = configuration.GetValue<string>("Jwt:Audience")
            ?? throw new InvalidOperationException("Jwt:Audience missing");

        _expiresHours = configuration.GetValue<int>("Jwt:ExpiresHours");

        if (_expiresHours <= 0)
            throw new InvalidOperationException("Invalid Jwt:ExpiresHours");
    }

    public string CreateToken(User user, IEnumerable<string>? roles = null)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
        };
        
        if(roles != null && roles.Any())
        {
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        }

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_expiresHours),
            signingCredentials: _creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}