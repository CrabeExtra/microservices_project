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
    public DbSet<SkillToSkillAccomplishmentEntity> SkillToSkillAccomplishments { get; set; }
    public DbSet<SkillAccomplishmentEntity> SkillAccomplishments { get; set; }

    public DbSet<SkillSynonymsEntity> SkillSynonyms { get; set; }

    // Constraints
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SkillEntity>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Name)
                .UseCollation("NOCASE");

            entity.HasIndex(u => u.Name)
                .IsUnique();
            entity.HasIndex(u => u.Type);
        });

        modelBuilder.Entity<SkillAccomplishmentEntity>(entity =>
        {
            entity.HasKey(u => u.Id);
        });

        modelBuilder.Entity<SkillSynonymsEntity>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Name)
                .UseCollation("NOCASE");
            entity.HasIndex(u => u.Name)
                .IsUnique(); // tempted to use different type of indexing. But I'm using SQLite and the dataset will always be 'not big data'.
            entity.HasIndex(u => u.SkillId);

            entity
                .HasOne(u => u.Skill)
                .WithMany(u => u.Synonyms)
                .HasForeignKey(u => u.SkillId);
        });

        modelBuilder.Entity<SkillToSkillAccomplishmentEntity>(entity =>
        {
            entity.HasKey(u => new { u.SkillId, u.SkillAccomplishmentId });

            entity.HasIndex(u => u.SkillAccomplishmentId);

            entity  
                .HasOne(u => u.Skill)
                .WithMany(u => u.SkillToAccomplishments)
                .HasForeignKey(u => u.SkillId);

            entity
                .HasOne(u => u.SkillAccomplishment)
                .WithMany(u => u.SkillsToAccomplishments)
                .HasForeignKey(u => u.SkillAccomplishmentId);

        });

    }

}