
using Identity.Database.Repository.Interface;
using Identity.Database.Repository;

namespace Identity.Database;

public static class DatabaseCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IUserRepository, UserRepository>(provider => new UserRepository(connectionString));
        return services;
    }
}