using Microsoft.EntityFrameworkCore;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Infrastructure;

public class MySqlDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<ImageEntity> Images { get; set; }
    
    private const string COLLATION = "latin1_bin";

    public MySqlDbContext(DbContextOptions<MySqlDbContext> options)
            : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // used to setup fields to be case-sensitive
        modelBuilder.UseCollation(COLLATION);

        // UserEntity
        modelBuilder.Entity<UserEntity>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<UserEntity>().HasIndex(u => u.Login).IsUnique();
        modelBuilder.Entity<UserEntity>().Property(u => u.IsActive).HasDefaultValue(true);

        // RefreshTokenEntity
        modelBuilder.Entity<RefreshTokenEntity>().Property(rt => rt.IsActive).HasDefaultValue(true);

        // Image Entity
        modelBuilder.Entity<ImageEntity>().HasIndex(u => u.ImageId).IsUnique();
    }
}
