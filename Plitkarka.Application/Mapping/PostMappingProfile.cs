using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Application.Mapping;

public class PostMappingProfile : Profile
{
    public PostMappingProfile()
    {
        CreateMap<Post, PostEntity>();
        CreateMap<PostEntity, Post>();
    }
}
