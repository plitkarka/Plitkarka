using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Models;

public record CommentLikeEntity : Entity
{
    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public UserEntity? User { get; set; }

    public Guid CommentId { get; set; }

    public CommentEntity? Comment { get; set; }
}
