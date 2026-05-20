using Identity.Application.Service;
using Identity.Application.Service.Interface;

namespace Identity.Application;

public static class ApplicationCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // program scoped services
        services.AddSingleton<IJwtSigningKeyService,JwtSigningKeyService>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IHashService, HashService>();

        // request scoped services
        services.AddScoped<IUserService, UserService>();
        
        
        return services;
    }
}