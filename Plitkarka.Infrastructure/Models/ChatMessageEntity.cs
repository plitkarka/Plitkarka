using Plitkarka.Commons.Helpers;
using Plitkarka.Infrastructure.Models.Abstractions;

namespace Plitkarka.Infrastructure.Models;

public record ChatMessageEntity : ActivatedEntity
{
    public string TextContent { get; set; }

    public bool VisibleToCreator { get; set; } = true;

    public MessageType Type { get; set; } = MessageType.Message;

    // ----- Relation properties -----

    public Guid ChatId { get; set; }

    public ChatEntity Chat { get; set; }

    public Guid UserId { get; set; }

    public UserEntity User { get; set; }

    public Guid? PostId { get; set; }

    public PostEntity? Post { get; set; }

    public Guid? ChatMessageImageId { get; set; }

    public ChatMessageImageEntity? ChatMessageImage { get; set; }
}
