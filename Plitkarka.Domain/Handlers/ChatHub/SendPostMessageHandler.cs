using AutoMapper;
using MediatR;
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

public class SendPostMessageHandler : 
    ChatSendingBase,
    IRequestHandler<SendPostMessageRequest, HubNotificationHandlerResponse>
{
    private IRepository<ChatMessageEntity> _messageRepository { get; init; }
    private IRepository<PostEntity> _postRepository { get; init; }

    public SendPostMessageHandler(
        IContextUserService contextUserService,
        IMapper mapper,
        IRepository<ChatEntity> chatRepository,
        IRepository<ChatUserConfigurationEntity> configurationRepository,
        IRepository<ChatMessageEntity> messageRepository,
        IRepository<HubConnectionEntity> connectionRepository,
        IRepository<PostEntity> postRepository)
    : base(
        contextUserService,
        mapper,
        chatRepository,
        configurationRepository,
        connectionRepository)
    {
        _messageRepository = messageRepository;
        _postRepository = postRepository;
    }

    public async Task<HubNotificationHandlerResponse> Handle(SendPostMessageRequest request, CancellationToken cancellationToken)
    {
        var chat = await GetChatWith(request.ReceiverId);

        if (await _postRepository.GetByIdAsync(request.PostId) == null)
        {
            throw new ValidationException("Post not found");
        }

        var message = new ChatMessage
        {
            Type = MessageType.PostMessage,
            TextContent = request.Message,
            ChatId = chat.Id,
            UserId = _user.Id,
            PostId = request.PostId
        };

        var messageId = await _messageRepository.AddAsync(
            _mapper.Map<ChatMessageEntity>(message));

        var result = new HubNotificationHandlerResponse
        {
            Notification = new HubNotification
            {
                Type = nameof(MessageType.PostMessage),
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
