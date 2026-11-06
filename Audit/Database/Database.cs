
using Audit.Database.Repository.Interface;
using Audit.Database.Repository;
using Audit.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Audit.Database;

public static class DatabaseCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<IRecordRepository, RecordRepository>();
        return services;
    }
}