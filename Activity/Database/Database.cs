
using Activity.Database.Repository.Interface;
using Activity.Database.Repository;
using Activity.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Activity.Database;

public static class DatabaseCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<INotificationRepository, NotificationRepository>(provider => new NotificationRepository(connectionString));
        return services;
    }
}