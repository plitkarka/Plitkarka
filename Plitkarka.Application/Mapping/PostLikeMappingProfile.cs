using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Application.Mapping;

public class PostLikeMappingProfile : Profile
{
    public PostLikeMappingProfile()
    {
        CreateMap<PostLike, PostLikeEntity>();
        CreateMap<PostLikeEntity, PostLike>();
    }
}
