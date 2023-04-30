using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Plitkarka.Infrastructure.ModelAbstractions;

namespace Plitkarka.Infrastructure.Models;

public record PostEntity : ActivatedEntity
{
    [MaxLength(500)]
    public string TextContent { get; set; }

    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public UserEntity? User { get; set; }

    public ICollection<PostLikeEntity> PostLikes { get; set; }

    public ICollection<CommentEntity> Comments { get; set; }
}
