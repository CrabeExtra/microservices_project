
using Microsoft.EntityFrameworkCore;
using Personal.Database.Context;
using Personal.Database.Repository.Interface;

namespace Personal.Database;

public static class DatabaseCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<ISkillRepository, SkillRepository>();

        return services;
    }
}