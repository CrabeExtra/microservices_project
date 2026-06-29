using Identity.Application.Service;
using Identity.Application.Service.Interface;
using Identity.Domain.Entity;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application;

public static class ApplicationCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // program scoped services
        services.AddSingleton<IJwtSigningKeyService, JwtSigningKeyService>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IHashService, HashService>();
        services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

        // request scoped services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        
        
        return services;
    }
}