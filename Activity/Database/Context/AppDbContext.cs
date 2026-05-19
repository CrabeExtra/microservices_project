using Microsoft.EntityFrameworkCore;
using Activity.Database.Entity;

namespace Activity.Database.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<NotificationEntity> Notifications { get; set; }
}