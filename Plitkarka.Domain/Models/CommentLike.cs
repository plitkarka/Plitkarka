using Plitkarka.Domain.Models.Abstractions;

namespace Plitkarka.Domain.Models;

public record CommentLike : LogicModel
{
    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public Guid CommentId { get; set; }
}
