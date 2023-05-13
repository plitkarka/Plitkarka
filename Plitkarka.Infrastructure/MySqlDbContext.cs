using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Infrastructure.ModelAbstractions;
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
    public DbSet<PostPinEntity> PostPins { get; set; }
    public DbSet<PostShareEntity> PostShares { get; set; }

    private const string Collation = "latin1_bin";

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
        modelBuilder
            .UseCollation(Collation);

        modelBuilder
            .SetupActivatedEntity<UserEntity>()
            .SetupActivatedEntity<RefreshTokenEntity>()
            .SetupActivatedEntity<ImageEntity>()
            .SetupActivatedEntity<PostEntity>()
            .SetupActivatedEntity<SubscriptionEntity>()
            .SetupActivatedEntity<SubscriptionEntity>()
            .SetupActivatedEntity<CommentEntity>()
            .SetupActivatedEntity<PostShareEntity>();

        modelBuilder
            .SetupEntity<CommentLikeEntity>()
            .SetupEntity<PostLikeEntity>()
            .SetupEntity<PostPinEntity>();

        // ImageEntity
        modelBuilder.Entity<ImageEntity>().HasIndex(e => e.ImageId).IsUnique();

        // SubscriptionEntities
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
    }
}
