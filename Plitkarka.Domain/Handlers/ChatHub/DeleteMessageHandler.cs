using MediatR;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Commons.Helpers;
using Plitkarka.Domain.Handlers.ChatHub.Abstract;
using Plitkarka.Domain.Requests.ChatHub;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.ChatHub;

public class DeleteMessageHandler :
    ChatBase,
    IRequestHandler<DeleteMessageRequest, HubNotificationHandlerResponse>
{
    private IRepository<ChatMessageEntity> _messageRepository { get; init; }

    public DeleteMessageHandler(
        IContextUserService contextUserService,
        IRepository<ChatMessageEntity> messageRepository,
        IRepository<HubConnectionEntity> connectionRepository)
    : base(
        contextUserService,
        connectionRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<HubNotificationHandlerResponse> Handle(DeleteMessageRequest request, CancellationToken cancellationToken)
    {
        ChatMessageEntity? message;

        try
        {
            message = await _messageRepository
                .GetAll()
                .Include(e => e.Chat)
                    .ThenInclude(e => e.ChatUserConfigurations)
                .FirstOrDefaultAsync(e => e.Id == request.MessageId);
        }
        catch(Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if(message == null)
        {
            throw new ValidationException("Message not found");
        }

        if(message.UserId != _user.Id)
        {
            throw new ValidationException("Authorized user has no permission to delete this message");
        }

        var chat = message.Chat;

        var result = new HubNotificationHandlerResponse
        {
            Notification = new HubNotification
            {
                Type = nameof(MessageType.DeleteMessage),
                SenderId = _user.Id,
                ObjectId = message.Id,
                SenderLogin = _user.Login,
                Message = ""
            }
        };

        await EnrichsReponseWithConnections(chat, result);

        await _messageRepository.DeleteAsync(message);

        return result;
    }
}
