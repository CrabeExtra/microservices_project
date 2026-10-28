
using Audit.Database.Repository.Interface;
using Audit.Database.Repository;

namespace Audit.Database;

public static class DatabaseCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IRecordRepository, RecordRepository>();
        return services;
    }
}