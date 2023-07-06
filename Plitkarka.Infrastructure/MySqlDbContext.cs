using Microsoft.EntityFrameworkCore;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Infrastructure;

public class MySqlDbContext : DbContext
{
    private const string Collation = "utf8mb4_bin";

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<PostImageEntity> PostImages { get; set; }
    public DbSet<UserImageEntity> UserImages { get; set; }
    public DbSet<ChatMessageImageEntity> ChatMessageImages { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<SubscriptionEntity> Subscriptions { get; set; }
    public DbSet<PostLikeEntity> PostLikes { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }
    public DbSet<CommentLikeEntity> CommentLikes { get; set; }
    public DbSet<PostPinEntity> PostPins { get; set; }
    public DbSet<PostShareEntity> PostShares { get; set; }
    public DbSet<HubConnectionEntity> HubConnections { get; set; }
    public DbSet<ChatEntity> Chats { get; set; }
    public DbSet<ChatUserConfigurationEntity> ChatUserConfigurations { get; set; }
    public DbSet<ChatMessageEntity> ChatMessages { get; set; }

    public MySqlDbContext(DbContextOptions<MySqlDbContext> options)
            : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // used to setup fields to be case-sensitive and allows different languages
        modelBuilder
            .UseCollation(Collation);

        modelBuilder
            .SetupActivatedEntity<UserEntity>()
            .SetupActivatedEntity<PostImageEntity>()
            .SetupActivatedEntity<UserImageEntity>()
            .SetupActivatedEntity<ChatMessageImageEntity>()
            .SetupActivatedEntity<PostEntity>()
            .SetupActivatedEntity<SubscriptionEntity>()
            .SetupActivatedEntity<SubscriptionEntity>()
            .SetupActivatedEntity<CommentEntity>()
            .SetupActivatedEntity<PostShareEntity>()
            .SetupActivatedEntity<ChatEntity>()
            .SetupActivatedEntity<ChatUserConfigurationEntity>()
            .SetupActivatedEntity<ChatMessageEntity>();

        modelBuilder
            .SetupEntity<RefreshTokenEntity>()
            .SetupEntity<CommentLikeEntity>()
            .SetupEntity<PostLikeEntity>()
            .SetupEntity<PostPinEntity>()
            .SetupEntity<HubConnectionEntity>();

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
        modelBuilder
            .Entity<UserImageEntity>()
            .HasIndex(e => e.ImageKey)
            .IsUnique();

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
        modelBuilder
            .Entity<PostImageEntity>()
            .HasIndex(e => e.ImageKey)
            .IsUnique();

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

        // ChatMessageImageEntity
        modelBuilder
           .Entity<ChatMessageImageEntity>()
           .HasIndex(e => e.ImageKey)
           .IsUnique();

        modelBuilder.Entity<ChatMessageImageEntity>()
            .HasOne(e => e.ChatMessage)
            .WithOne(e => e.ChatMessageImage)
            .HasForeignKey<ChatMessageEntity>(e => e.ChatMessageImageId)
            .IsRequired(false);

        modelBuilder.Entity<ChatMessageEntity>()
            .HasOne(e => e.ChatMessageImage)
            .WithOne(e => e.ChatMessage)
            .HasForeignKey<ChatMessageImageEntity>(e => e.ChatMessageId)
            .IsRequired(false);

        // ChatEntity
        modelBuilder.Entity<ChatEntity>()
            .Property(e => e.LastUpdateTime).HasDefaultValueSql(ModelBuilderExtensions.DateFunction);

        // ChatUserConfigurationEntity
        modelBuilder.Entity<ChatUserConfigurationEntity>()
            .Property(e => e.ChatDeletedTime).HasDefaultValueSql(ModelBuilderExtensions.DateFunction);

        modelBuilder.Entity<ChatUserConfigurationEntity>()
            .Property(e => e.LastOpenedTime).HasDefaultValueSql(ModelBuilderExtensions.DateFunction);
    }
}
