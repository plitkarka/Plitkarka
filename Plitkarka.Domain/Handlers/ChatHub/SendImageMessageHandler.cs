using AutoMapper;
using MediatR;
using Plitkarka.Commons.Helpers;
using Plitkarka.Domain.Handlers.ChatHub.Abstract;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.ChatHub;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.ImageService;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.ChatHub;

public class SendImageMessageHandler : 
    ChatSendingBase,
    IRequestHandler<SendImageMessageRequest, HubNotificationHandlerResponse>
{
    private IRepository<ChatMessageEntity> _messageRepository { get; init; }
    private IRepository<ChatMessageImageEntity> _imageRepository { get; init; }
    private IImageService _imageService { get; init; }

    public SendImageMessageHandler(
        IContextUserService contextUserService,
        IMapper mapper,
        IRepository<ChatEntity> chatRepository,
        IRepository<ChatUserConfigurationEntity> configurationRepository,
        IRepository<ChatMessageEntity> messageRepository,
        IRepository<HubConnectionEntity> connectionRepository,
        IRepository<ChatMessageImageEntity> imageRepository,
        IImageService imageService) 
    : base(
        contextUserService,
        mapper,
        chatRepository,
        configurationRepository,
        connectionRepository)
    {
        _messageRepository = messageRepository;
        _imageService = imageService;
        _imageRepository = imageRepository;
    }

    public async Task<HubNotificationHandlerResponse> Handle(SendImageMessageRequest request, CancellationToken cancellationToken)
    {
        var imageKey = await _imageService.UploadImageAsync(request.Image);

        var chat = await GetChatWith(request.ReceiverId);

        var message = new ChatMessage
        {
            Type = MessageType.ImageMessage,
            TextContent = request.Message,
            ChatId = chat.Id,
            UserId = _user.Id
        };

        var messageEntity = _mapper.Map<ChatMessageEntity>(message);

        var messageId = await _messageRepository.AddAsync(messageEntity);

        var image = new ChatMessageImageEntity
        {
            ImageKey = imageKey,
            ChatMessageId = messageId
        };

        messageEntity.ChatMessageImageId = await _imageRepository.AddAsync(image);

        await _messageRepository.UpdateAsync(messageEntity);

        var result = new HubNotificationHandlerResponse
        {
            Notification = new HubNotification
            {
                Type = nameof(MessageType.ImageMessage),
                SenderId = _user.Id,
                ObjectId = messageId,
                SenderLogin = _user.Login,
                Message = request.Message
            }
        };

        await EnrichsReponseWithConnections(chat, result);

        return result;
    }
}
