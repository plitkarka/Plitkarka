using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Application.Mapping;

public class PostPinMappingProfile : Profile
{
    public PostPinMappingProfile()
    {
        CreateMap<PostPin, PostPinEntity>();
        CreateMap<PostPinEntity, PostPin>();
    }
}
