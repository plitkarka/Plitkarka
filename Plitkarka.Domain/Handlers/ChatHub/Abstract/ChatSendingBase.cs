using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Plitkarka.Commons.Exceptions;
using Plitkarka.Domain.Models;
using Plitkarka.Domain.Services.ContextUser;
using Plitkarka.Infrastructure.Models;
using Plitkarka.Infrastructure.Services;

namespace Plitkarka.Domain.Handlers.ChatHub.Abstract;

public abstract class ChatSendingBase : ChatBase
{
    protected IMapper _mapper { get; init; }
    protected IRepository<ChatEntity> _chatRepository { get; init; }
    protected IRepository<ChatUserConfigurationEntity> _configurationRepository { get; init; }

    public ChatSendingBase(
        IContextUserService contextUserService,
        IMapper mapper,
        IRepository<ChatEntity> chatRepository,
        IRepository<ChatUserConfigurationEntity> configurationRepository,
        IRepository<HubConnectionEntity> connectionRepository)
    : base (
        contextUserService, 
        connectionRepository)
    {
        _mapper = mapper;
        _chatRepository = chatRepository;
        _configurationRepository = configurationRepository;
    }

    public async Task<ChatEntity> GetChatWith(Guid receiverId)
    {
        ChatEntity? chatEntity;

        try
        {
            chatEntity = await _chatRepository
                .GetAll()
                .Include(e => e.ChatUserConfigurations)
                .FirstOrDefaultAsync(entity =>
                entity.ChatUserConfigurations.Count == 2 &&
                entity.ChatUserConfigurations.Any(conf => conf.UserId == _user.Id) &&
                entity.ChatUserConfigurations.Any(conf => conf.UserId == receiverId));
        }
        catch (Exception ex)
        {
            throw new MySqlException(ex.Message);
        }

        if (chatEntity != null)
        {
            chatEntity.LastUpdateTime = DateTime.Now;
            return await _chatRepository.UpdateAsync(chatEntity);
        }

        var chat = new Chat
        {
            LastUpdateTime = DateTime.UtcNow,
        };

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
}
