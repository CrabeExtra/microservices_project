using Microsoft.EntityFrameworkCore;
using Audit.Database.Entity;

namespace Audit.Database.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Entities 
    public DbSet<RecordEntity> Records { get; set; }

    // Constraints
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RecordEntity>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.HasIndex(u => u.CreatedAt);
            entity.HasIndex(u => u.MicroserviceName);
            entity.HasIndex(u => u.EntityName);
            entity.HasIndex(u => u.Action);
            entity.HasIndex(u => u.EventType);
            entity.HasIndex(u => u.ReferenceId);
        });
    }

}