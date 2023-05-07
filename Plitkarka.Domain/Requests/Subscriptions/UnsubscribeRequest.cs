using MediatR;

namespace Plitkarka.Domain.Requests.Subscriptions;

public record UnsubscribeRequest(Guid UnsubscribeFromId)
    : IRequest;
