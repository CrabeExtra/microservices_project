


using Contact.Application.Service;
using Contact.Application.Service.Interface;

namespace Contact.Application;

public static class ApplicationCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // program scoped services
        services.AddSingleton<IApiService, ApiService>();

        // request scoped services
        services.AddScoped<IInteractionService, InteractionService>();

        // extras
        services.AddHttpClient<ApiService>();
    
        return services;
    }
}