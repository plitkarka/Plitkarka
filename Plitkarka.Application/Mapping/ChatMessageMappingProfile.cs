using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Application.Mapping;

public class ChatMessageMappingProfile : Profile
{
    public ChatMessageMappingProfile()
    {
        CreateMap<ChatMessage, ChatMessageEntity>();
        CreateMap<ChatMessageEntity, ChatMessage>();
    }
}
