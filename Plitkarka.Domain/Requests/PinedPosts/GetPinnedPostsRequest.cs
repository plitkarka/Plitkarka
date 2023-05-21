using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.PinedPosts;

public record GetPinnedPostsRequest(
    int Page,
    Guid UserId)
    : IRequest<PaginationResponse<PostResponse>>;
