using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Models;

public record HubConnectionEntity : Entity
{
    public string ConnectionId { get; set; }

    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public UserEntity User { get; set; }
}
