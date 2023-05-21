using Plitkarka.Domain.Models.Abstractions;

namespace Plitkarka.Domain.Models;

public record Subscription : ActivatedLogicModel
{
    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public Guid SubscribedToId { get; set; }
}
