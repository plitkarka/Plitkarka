using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Application.Mapping;

public class HubConnectionMappingProfile : Profile
{
    public HubConnectionMappingProfile()
    {
        CreateMap<HubConnection, HubConnectionEntity>();
        CreateMap<HubConnectionEntity, HubConnection>();
    }
}
