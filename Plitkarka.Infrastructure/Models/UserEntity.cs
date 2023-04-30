using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plitkarka.Infrastructure.ModelAbstractions;

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

    public DateTime BirthDate { get; set; }

    public DateTime LastLoginDate { get; set; }


    // ----- Relation properties -----

    public Guid? RefreshTokenId { get; set; }

    public RefreshTokenEntity? RefreshToken { get; set; }

    public ICollection<PostEntity> Posts { get; set; }

    public ICollection<PostLikeEntity> PostLikes { get; set; }

    public ICollection<CommentEntity> Comments { get; set; }

    public ICollection<CommentLikeEntity> CommentLikes { get; set; }

    public ICollection<SubscriptionEntity> Subscriptions { get; set; }

    public ICollection<SubscriptionEntity> Subscribers { get; set; }
}
