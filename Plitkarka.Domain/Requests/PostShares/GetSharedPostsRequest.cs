using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.PostShares;

public record GetSharedPostsRequest(
    int Page,
    Guid UserId)
    : IRequest<PaginationResponse<PostResponse>>;
