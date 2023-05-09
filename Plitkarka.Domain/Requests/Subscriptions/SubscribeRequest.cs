using MediatR;

namespace Plitkarka.Domain.Requests.Subscriptions;

public record SubscribeRequest(Guid SusbcribeToId) 
    : IRequest<Guid>;