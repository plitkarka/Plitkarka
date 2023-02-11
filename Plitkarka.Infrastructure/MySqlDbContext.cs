using Microsoft.EntityFrameworkCore;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Infrastructure;

public class MySqlDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }

    public MySqlDbContext(DbContextOptions<MySqlDbContext> options)
            : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<UserEntity>().HasIndex(u => u.Login).IsUnique();
    }
}
