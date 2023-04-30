using Microsoft.EntityFrameworkCore;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Infrastructure;

public class MySqlDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<ImageEntity> Images { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<SubscriptionEntity> Subscriptions { get; set; }
    public DbSet<PostLikeEntity> PostLikes { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }
    public DbSet<CommentLikeEntity> CommentLikes { get; set; }

    private const string Collation = "latin1_bin";
    private const string DateFunction = "(CURRENT_TIMESTAMP)";

    public MySqlDbContext(DbContextOptions<MySqlDbContext> options)
            : base(options)
    {
        // Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // used to setup fields to be case-sensitive
        modelBuilder.UseCollation(Collation);

        // UserEntity
        modelBuilder.Entity<UserEntity>().Property(e => e.CreationTime).HasDefaultValueSql(DateFunction);
        modelBuilder.Entity<UserEntity>().Property(e => e.IsActive).HasDefaultValue(true);

        // RefreshTokenEntity
        modelBuilder.Entity<RefreshTokenEntity>().Property(e => e.CreationTime).HasDefaultValueSql(DateFunction);
        modelBuilder.Entity<RefreshTokenEntity>().Property(e => e.IsActive).HasDefaultValue(true);

        // ImageEntity
        modelBuilder.Entity<ImageEntity>().Property(e => e.CreationTime).HasDefaultValueSql(DateFunction);
        modelBuilder.Entity<ImageEntity>().Property(e => e.IsActive).HasDefaultValue(true);
        modelBuilder.Entity<ImageEntity>().HasIndex(e => e.ImageId).IsUnique();

        // PostEntity
        modelBuilder.Entity<PostEntity>().Property(e => e.CreationTime).HasDefaultValueSql(DateFunction);
        modelBuilder.Entity<PostEntity>().Property(e => e.IsActive).HasDefaultValue(true);

        // SubscriptionEntities
        modelBuilder.Entity<SubscriptionEntity>().Property(e => e.CreationTime).HasDefaultValueSql(DateFunction);
        modelBuilder.Entity<SubscriptionEntity>().Property(e => e.IsActive).HasDefaultValue(true);

        modelBuilder
            .Entity<SubscriptionEntity>()
            .HasOne(se => se.SubscribedTo)
            .WithMany(ue => ue.Subscribers)
            .HasForeignKey(se => se.SubscribedToId);

        modelBuilder
            .Entity<SubscriptionEntity>()
            .HasOne(se => se.User)
            .WithMany(ue => ue.Subscriptions)
            .HasForeignKey(se => se.UserId);

        // CommentEntity
        modelBuilder.Entity<CommentEntity>().Property(e => e.CreationTime).HasDefaultValueSql(DateFunction);
        modelBuilder.Entity<CommentEntity>().Property(e => e.IsActive).HasDefaultValue(true);
    }
}
