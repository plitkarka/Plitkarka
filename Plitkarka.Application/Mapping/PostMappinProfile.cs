using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Application.Mapping;

public class PostMappinProfile : Profile
{
    public PostMappinProfile()
    {
        CreateMap<Post, PostEntity>();
        CreateMap<PostEntity, Post>();
    }
}
