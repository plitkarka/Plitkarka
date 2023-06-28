using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Application.Mapping;

public class ChatUserConfigurationMappinfProfile : Profile
{
    public ChatUserConfigurationMappinfProfile()
    {
        CreateMap<ChatUserConfiguration, ChatUserConfigurationEntity>();
        CreateMap<ChatUserConfigurationEntity, ChatUserConfiguration>();
    }
}
