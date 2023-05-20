using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Models;

public record SubscriptionEntity : ActivatedEntity
{
    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public UserEntity User { get; set; }

    public Guid SubscribedToId { get; set; }

    public UserEntity SubscribedTo { get; set; }
}
