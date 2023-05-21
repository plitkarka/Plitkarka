using System.ComponentModel.DataAnnotations;
using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Models;

public record PostEntity : ActivatedEntity
{
    [MaxLength(500)]
    public string? TextContent { get; set; }

    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public UserEntity? User { get; set; }

    public Guid PostImageId { get; set; }

    public PostImageEntity? PostImage { get; set; }

    public ICollection<PostLikeEntity> PostLikes { get; set; }

    public ICollection<CommentEntity> Comments { get; set; }

    public ICollection<PostPinEntity> Pins { get; set; }

    public ICollection<PostShareEntity> Shares { get; set; }
}
