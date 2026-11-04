using Microsoft.EntityFrameworkCore;
using Personal.Database.Entity;

namespace Personal.Database.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Entities 

    public DbSet<SkillEntity> Skills { get; set; }
    public DbSet<SkillAccomplishmentEntity> SkillAccomplishments { get; set; }

    public DbSet<SkillSynonymsEntity> SkillSynonyms { get; set; }

    // Constraints
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SkillEntity>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Name)
                .IsUnique();
            entity.HasIndex(u => u.Type);
        });

        modelBuilder.Entity<SkillAccomplishmentEntity>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.SkillId); // always will be searched by SkillId
        });

        modelBuilder.Entity<SkillSynonymsEntity>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Name)
                .IsUnique(); // tempted to use different type of indexing. But I'm using SQLite and the dataset will always be 'not big data'.
            entity.HasIndex(u => u.SkillId);
        });

    }

}