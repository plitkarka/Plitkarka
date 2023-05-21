using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.Subscriptions;

public record GetSubscribersRequest(
    int Page,
    Guid UserId) 
    : IRequest<PaginationResponse<UserPreviewResponse>>;
