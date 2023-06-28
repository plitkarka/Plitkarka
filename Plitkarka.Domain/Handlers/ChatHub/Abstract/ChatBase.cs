using Microsoft.EntityFrameworkCore;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.ChatHub.Abstract;

public abstract class ChatBase
{
    protected User _user { get; init; }
    protected IRepository<HubConnectionEntity> _connectionRepository { get; init; }

    public ChatBase(
        IContextUserService contextUserService,
        IRepository<HubConnectionEntity> connectionRepository)
    {
        _user = contextUserService.User;
        _connectionRepository = connectionRepository;
    }

    public async Task EnrichsReponseWithConnections(ChatEntity chat, HubNotificationHandlerResponse result)
    {
        if (result.ConnectionIds == null)
        {
            result.ConnectionIds = new List<string>();
        }

        foreach (var config in chat.ChatUserConfigurations)
        {
            if (config.UserId == _user.Id)
            {
                continue;
            }

            var connections = await _connectionRepository.GetAll().Where(
                entity => entity.UserId == config.UserId).ToListAsync();

            foreach (var connection in connections)
            {
                result.ConnectionIds.Add(connection.ConnectionId);
            }
        }
    }
}
