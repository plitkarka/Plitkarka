using Plitkarka.Domain.Models.Abstractions;

namespace Plitkarka.Domain.Models;

public record PostPin : LogicModel
{
    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public Guid PostId { get; set; }
}
