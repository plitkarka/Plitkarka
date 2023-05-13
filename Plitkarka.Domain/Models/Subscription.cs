namespace Plitkarka.Domain.Models;

public class Subscription
{
    public Guid Id { get; set; }

    public bool IsActive { get; set; }

    public Guid UserId { get; set; }

    public Guid SubscribedToId { get; set; }
}
