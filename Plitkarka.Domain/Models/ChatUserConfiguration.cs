using Plitkarka.Domain.Models.Abstractions;

namespace Plitkarka.Domain.Models;

public record ChatUserConfiguration : ActivatedLogicModel
{
    public DateTime ChatDeletedTime { get; set; }

    public DateTime LastOpenedTime { get; set; }

    // ----- Relation properties -----

    public Guid ChatId { get; set; }

    public Guid UserId { get; set; }
}
