
namespace Contact.Database;

public static class DatabaseCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        return services;
    }
}