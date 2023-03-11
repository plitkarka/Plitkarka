using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Application.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserEntity>();
        CreateMap<UserEntity, User>();
    }
}
