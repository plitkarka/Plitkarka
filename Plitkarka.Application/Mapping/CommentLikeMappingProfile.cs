using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Application.Mapping;

public class CommentLikeMappingProfile : Profile
{
    public CommentLikeMappingProfile()
    {
        CreateMap<CommentLike, CommentLikeEntity>();
        CreateMap<CommentLikeEntity, CommentLike>();
    }
}
