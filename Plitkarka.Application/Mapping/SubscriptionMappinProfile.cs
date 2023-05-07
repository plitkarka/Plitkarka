using AutoMapper;
using Plitkarka.Domain.Models;
using Plitkarka.Infrastructure.Models;

namespace Plitkarka.Application.Mapping;

public class SubscriptionMappinProfile : Profile
{
    public SubscriptionMappinProfile()
    {
        CreateMap<Subscription, SubscriptionEntity>();
        CreateMap<SubscriptionEntity, Subscription>();
    }
}
