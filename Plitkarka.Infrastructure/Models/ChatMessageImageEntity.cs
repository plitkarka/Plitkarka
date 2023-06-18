using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Models;

public record ChatMessageImageEntity : ImageEntity
{
    public Guid? ChatMessageId { get; set; }

    public ChatMessageEntity? ChatMessage { get; set; }
}
