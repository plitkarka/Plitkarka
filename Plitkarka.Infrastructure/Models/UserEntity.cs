using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Models;

public record UserEntity : ActivatedEntity
{
    [Required]
    [MaxLength(20)]
    public string Login { get; set; }

    [Required]
    [MaxLength(20)]
    public string Name { get; set; }

    [Required]
    [MaxLength(255)]
    public string Email { get; set; }

    [MaxLength(6)]
    public string EmailCode { get; set; }

    [Required]
    [Column(TypeName="char(64)")]
    public string Password { get; set; }

    public int PasswordAttempts { get; set; }

    [Required]
    [Column(TypeName = "char(64)")]
    public string Salt { get; set; }

    [MaxLength(255)]
    public string? Description { get; set; }

    [MaxLength(255)]
    public string? Link { get; set; }

    public DateTime BirthDate { get; set; }

    public DateTime LastLoginDate { get; set; }

    [MaxLength(6)]
    public string? ChangePasswordCode { get; set; }

    // ----- Relation properties -----

    public ICollection<RefreshTokenEntity> RefreshTokens { get; set; }

    public Guid? UserImageId { get; set; }

    public UserImageEntity? UserImage { get; set; }

    public ICollection<PostEntity> Posts { get; set; }

    public ICollection<PostLikeEntity> PostLikes { get; set; }

    public ICollection<CommentEntity> Comments { get; set; }

    public ICollection<CommentLikeEntity> CommentLikes { get; set; }

    public ICollection<SubscriptionEntity> Subscriptions { get; set; }

    public ICollection<SubscriptionEntity> Subscribers { get; set; }

    public ICollection<ChatUserConfigurationEntity> ChatUserConfigurations { get; set; }

    public ICollection<HubConnectionEntity> Connections { get; set; }

    public ICollection<ChatMessageEntity> Messages { get; set; }
}
