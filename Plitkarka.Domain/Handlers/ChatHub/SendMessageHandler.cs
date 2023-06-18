using AutoMapper;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Helpers;
using Plitkarka.Domain.Handlers.ChatHub.Abstract;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Requests.ChatHub;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.ChatHub;

public class SendMessageHandler : 
    ChatSendingBase,
    IRequestHandler<SendMessageRequest, HubNotificationHandlerResponse>
{
    private IRepository<ChatMessageEntity> _messageRepository { get; init; }

    public SendMessageHandler(
        IContextUserService contextUserService,
        IMapper mapper,
        IRepository<ChatEntity> chatRepository,
        IRepository<ChatUserConfigurationEntity> configurationRepository,
        IRepository<ChatMessageEntity> messageRepository,
        IRepository<HubConnectionEntity> connectionRepository)
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
            Notification = new HubNotification
            {
                Type = nameof(MessageType.Message),
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
