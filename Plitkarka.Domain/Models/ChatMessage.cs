using Plitkarka.Commons.Helpers;
using Plitkarka.Domain.Models.Abstractions;

namespace Plitkarka.Domain.Models;

public record ChatMessage : ActivatedLogicModel
{
    public string TextContent { get; set; }

    public MessageType Type { get; set; } = MessageType.Message;

    // ----- Relation properties -----

    public Guid ChatId { get; set; }

    public Guid UserId { get; set; }

    public Guid? PostId { get; set; }

    public Guid? ChatMessageImageId { get; set; }
}
