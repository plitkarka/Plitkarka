using Plitkarka.Domain.Models.Abstractions;

namespace Plitkarka.Domain.Models;

public record Post : ActivatedLogicModel
{
    public string? TextContent { get; set; }

    // ----- Relation properties -----

    public Guid UserId { get; set; }

    public Guid ImageId { get; set; }
}
