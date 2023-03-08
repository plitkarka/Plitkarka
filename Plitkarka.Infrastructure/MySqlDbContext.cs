using Microsoft.EntityFrameworkCore;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Infrastructure;

public class MySqlDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
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

        modelBuilder.Entity<UserEntity>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<UserEntity>().HasIndex(u => u.Login).IsUnique();

        modelBuilder.Entity<ImageEntity>().HasIndex(u => u.ImageId).IsUnique();
    }
}
