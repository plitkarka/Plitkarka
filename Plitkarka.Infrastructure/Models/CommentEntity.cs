using System.ComponentModel.DataAnnotations;
using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Models;

public record CommentEntity : ActivatedEntity
{
    [MaxLength(100)]
    public string TextContent { get; set; }

    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public UserEntity? User { get; set; }

    public Guid PostId { get; set; }

    public PostEntity? Post { get; set; }

    public ICollection<CommentLikeEntity> CommentLikes { get; set; }
}
