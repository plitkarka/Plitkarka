using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Models;

public record ChatUserConfigurationEntity : ActivatedEntity
{
    public DateTime ChatDeletedTime { get; set; }

    public DateTime LastOpenedTime { get; set; }

    // ----- Relation properties -----

    public Guid ChatId { get; set; }

    public ChatEntity Chat { get; set; }

    public Guid UserId { get; set; }

    public UserEntity User { get; set; }
}
