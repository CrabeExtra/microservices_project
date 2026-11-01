


using Personal.Application.Service;
using Personal.Application.Service.Interface;

namespace Personal.Application;

public static class ApplicationCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // program scoped services

        // request scoped services
        services.AddScoped<ISkillsService, SkillsService>();
    
        return services;
    }
}