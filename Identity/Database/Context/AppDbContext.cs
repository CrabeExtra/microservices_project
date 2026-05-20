using Microsoft.EntityFrameworkCore;
using Identity.Database.Entity;

namespace Identity.Database.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Entities 
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<UserRoleEntity> UserRoles { get; set; }

    // Constraints
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.HasIndex(u => u.Username);

            entity.Property(u => u.Email)
                .IsRequired();
        });

        modelBuilder.Entity<RoleEntity>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Name)
                .IsRequired();

            entity.HasIndex(r => r.Name)
                .IsUnique();
        });

        modelBuilder.Entity<UserRoleEntity>(entity =>
        {
            // composite primary key
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });

            // foreign key: User
            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // foreign key: Role
            entity.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

}