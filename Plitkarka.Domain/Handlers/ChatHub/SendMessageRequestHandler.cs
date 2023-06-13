using AutoMapper;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Helpers;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.ChatHub;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Domain.Services.ImageService;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.ChatHub;

public class SendMessageRequestHandler : ChatBase, IRequestHandler<SendMessageRequest, HubNotificationHandlerResponse>
{
    private IRepository<ChatMessageEntity> _messageRepository { get; init; }

    public SendMessageRequestHandler(
        IContextUserService contextUserService,
        IMapper mapper,
        IRepository<ChatEntity> chatRepository,
        IRepository<ChatUserConfigurationEntity> configurationRepository,
        IRepository<ChatMessageEntity> messageRepository,
        IRepository<HubConnectionEntity> connectionRepository, 
        IImageService imageService)
    : base(
        contextUserService,
        mapper,
        chatRepository,
        configurationRepository,
        connectionRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<HubNotificationHandlerResponse> Handle(SendMessageRequest request, CancellationToken cancellationToken)
    {
        if (request.Message.IsNullOrEmpty())
        {
            throw new ValidationException("Message is empty");
        }

        var chat = await GetChatWith(request.ReceiverId);

        var message = new ChatMessage
        {
            TextContent = request.Message,
            ChatId = chat.Id,  
            UserId = _user.Id
        };

        var messageId = await _messageRepository.AddAsync(
            _mapper.Map<ChatMessageEntity>(message));

        var result = new HubNotificationHandlerResponse
        {
            ConnectionIds = new List<string>(),
            Notification = new HubNotification
            {
                Type = nameof(MessageType.Message),
                SenderId = _user.Id,
                ObjectId = messageId,
                SenderLogin = _user.Login,
                Message = request.Message
            }
        };

        await AddConnections(chat, result);

        return result;
    }
}
