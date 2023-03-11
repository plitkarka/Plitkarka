using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Application.Mapping;

public class RefreshTokenMappingProfile : Profile
{
    public RefreshTokenMappingProfile()
    {
        CreateMap<RefreshToken, RefreshTokenEntity>();
        CreateMap<RefreshTokenEntity, RefreshToken>();
    }
}
