using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.ResponseModels;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.ChatHub;

public abstract class ChatBase
{
    protected User _user { get; init; }
    protected IMapper _mapper { get; init; }
    protected IRepository<ChatEntity> _chatRepository { get; init; }
    protected IRepository<ChatUserConfigurationEntity> _configurationRepository { get; init; }
    protected IRepository<HubConnectionEntity> _connectionRepository { get; init; }

    public ChatBase(
        IContextUserService contextUserService,
        IMapper mapper,
        IRepository<ChatEntity> chatRepository,
        IRepository<ChatUserConfigurationEntity> configurationRepository,
        IRepository<HubConnectionEntity> connectionRepository)
    {
        _user = contextUserService.User;
        _mapper = mapper;   
        _chatRepository = chatRepository;
        _configurationRepository = configurationRepository;
        _connectionRepository = connectionRepository;
    }

    public async Task<ChatEntity> GetChatWith(Guid receiverId)
    {
        ChatEntity? chatEntity;

        try
        {
            chatEntity = await _chatRepository.GetAll().FirstOrDefaultAsync(entity =>
                entity.ChatUserConfigurations.Count == 2 &&
                entity.ChatUserConfigurations.Any(user => user.Id == _user.Id) &&
                entity.ChatUserConfigurations.Any(user => user.Id == receiverId));
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (chatEntity != null)
        {
            return chatEntity;
        }

        var chat = new Chat();

        var chatId = await _chatRepository.AddAsync(
            _mapper.Map<ChatEntity>(chat));

        await CreateConfiguration(_user.Id, chatId);
        await CreateConfiguration(receiverId, chatId);

        chatEntity = await _chatRepository.GetByIdAsync(chatId);

        return chatEntity!;
    }

    private async Task CreateConfiguration(Guid userId, Guid chatId)
    {
        var configuration = new ChatUserConfiguration
        {
            UserId = userId,
            ChatId = chatId
        };

        await _configurationRepository.AddAsync(
            _mapper.Map<ChatUserConfigurationEntity>(configuration));
    }

    public async Task AddConnections(ChatEntity chat, HubNotificationHandlerResponse result)
    {
        foreach(var config in chat.ChatUserConfigurations)
        {
            if (config.UserId == _user.Id)
            {
                continue;
            }

            var connections = await _connectionRepository.GetAll().Where(
                entity => entity.UserId == config.UserId).ToListAsync();

            foreach(var connection in connections)
            {
                result.ConnectionIds.Add(connection.ConnectionId);
            }
        }
    }
}
