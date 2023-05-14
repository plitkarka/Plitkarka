using Plitkarka.Domain.Models.Abstractions;

namespace Plitkarka.Domain.Models;

public record PostLike : LogicModel
{
    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public Guid PostId { get; set; }
}
