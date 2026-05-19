using User.Application.Service;
using User.Application.Service.Interface;

namespace User.Application;

public static class ApplicationCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}