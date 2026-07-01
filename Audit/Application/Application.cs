

using Audit.Application.Service;
using Audit.Application.Service.Interface;

namespace Audit.Application;

public static class ApplicationCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // program scoped services
        //services.AddSingleton<IJwtSigningKeyService, JwtSigningKeyService>();

        // request scoped services
        services.AddScoped<IRecordService, RecordService>();

        return services;
    }
}