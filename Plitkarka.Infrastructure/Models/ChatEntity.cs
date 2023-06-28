using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Models;

public record ChatEntity : ActivatedEntity
{
    public DateTime LastUpdateTime { get; set; }

    // ----- Relation properties -----

    public ICollection<ChatUserConfigurationEntity> ChatUserConfigurations { get; set; }

    public ICollection<ChatMessageEntity> Messages { get; set; }
}
