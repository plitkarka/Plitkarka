namespace Plitkarka.Domain.Models;

public record PostPin
{
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public Guid PostId { get; set; }
}
