using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.Subscriptions;

public record GetSubscriptionsRequest(
    int Page,
    Guid UserId)
    : IRequest<PaginationResponse<UserPreviewResponse>>;
