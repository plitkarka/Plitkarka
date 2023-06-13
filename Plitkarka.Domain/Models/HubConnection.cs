using Plitkarka.Domain.Models.Abstractions;

namespace Plitkarka.Domain.Models;

public record HubConnection : LogicModel
{
    public string ConnectionId { get; set; }

    public Guid UserId { get; set; }
}
