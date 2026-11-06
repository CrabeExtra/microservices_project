
using Identity.Database.Repository.Interface;
using Identity.Database.Repository;
using Identity.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Identity.Database;

public static class DatabaseCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}