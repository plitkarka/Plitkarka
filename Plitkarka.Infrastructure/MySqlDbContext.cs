using Microsoft.EntityFrameworkCore;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Infrastructure;

public class MySqlDbContext : DbContext
{
    private static bool ShouldDelete = false;
    private static bool Deleted = false;

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<PostImageEntity> PostImages { get; set; }
    public DbSet<UserImageEntity> UserImages { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<SubscriptionEntity> Subscriptions { get; set; }
    public DbSet<PostLikeEntity> PostLikes { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }
    public DbSet<CommentLikeEntity> CommentLikes { get; set; }
    public DbSet<PostPinEntity> PostPins { get; set; }
    public DbSet<PostShareEntity> PostShares { get; set; }

    private const string Collation = "latin1_bin";

    public MySqlDbContext(DbContextOptions<MySqlDbContext> options)
            : base(options)
    {
        if (ShouldDelete)
        {
            if (!Deleted)
            {
                Database.EnsureDeleted();
                Deleted = true;
            }
        }

        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // used to setup fields to be case-sensitive
        modelBuilder
            .UseCollation(Collation);

        modelBuilder
            .SetupActivatedEntity<UserEntity>()
            .SetupActivatedEntity<RefreshTokenEntity>()
            .SetupActivatedEntity<PostImageEntity>()
            .SetupActivatedEntity<UserImageEntity>()
            .SetupActivatedEntity<PostEntity>()
            .SetupActivatedEntity<SubscriptionEntity>()
            .SetupActivatedEntity<SubscriptionEntity>()
            .SetupActivatedEntity<CommentEntity>()
            .SetupActivatedEntity<PostShareEntity>();

        modelBuilder
            .SetupEntity<CommentLikeEntity>()
            .SetupEntity<PostLikeEntity>()
            .SetupEntity<PostPinEntity>();

        // PostImageEntity
        modelBuilder.Entity<PostImageEntity>().HasIndex(e => e.ImageKey).IsUnique();

        // UserImageEntity
        modelBuilder.Entity<UserImageEntity>().HasIndex(e => e.ImageKey).IsUnique();

        // SubscriptionEntities
        modelBuilder
            .Entity<SubscriptionEntity>()
            .HasOne(e => e.SubscribedTo)
            .WithMany(e => e.Subscribers)
            .HasForeignKey(e => e.SubscribedToId);

        modelBuilder
            .Entity<SubscriptionEntity>()
            .HasOne(e => e.User)
            .WithMany(e => e.Subscriptions)
            .HasForeignKey(e => e.UserId);

        // UserImageEntity
        modelBuilder.Entity<UserImageEntity>()
            .HasOne(e => e.User)
            .WithOne(e => e.UserImage)
            .HasForeignKey<UserEntity>(e => e.UserImageId)
            .IsRequired(false);

        modelBuilder.Entity<UserEntity>()
            .HasOne(e => e.UserImage)
            .WithOne(e => e.User)
            .HasForeignKey<UserImageEntity>(e => e.UserId)
            .IsRequired(false);

        // PostImageEntity
        modelBuilder.Entity<PostImageEntity>()
            .HasOne(e => e.Post)
            .WithOne(e => e.PostImage)
            .HasForeignKey<PostEntity>(e => e.PostImageId)
            .IsRequired(false);

        modelBuilder.Entity<PostEntity>()
            .HasOne(e => e.PostImage)
            .WithOne(e => e.Post)
            .HasForeignKey<PostImageEntity>(e => e.PostId)
            .IsRequired(false);
    }
}
