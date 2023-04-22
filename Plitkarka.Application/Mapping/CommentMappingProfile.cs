using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Application.Mapping;

public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        CreateMap<Comment, CommentEntity>();
        CreateMap<CommentEntity, Comment>();
    }
}
