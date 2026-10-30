
using Activity.Database.Repository.Interface;
using Activity.Database.Repository;

namespace Activity.Database;

public static class DatabaseCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<INotificationRepository, NotificationRepository>(provider => new NotificationRepository(connectionString));
        return services;
    }
}