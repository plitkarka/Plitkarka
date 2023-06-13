using Plitkarka.Domain.Models.Abstractions;

namespace Plitkarka.Domain.Models;

public record HubConnection : LogicModel
{
    public string ConnectionId { get; set; }

    // ----- Relation properties -----

    public Guid UserId { get; set; }
}
