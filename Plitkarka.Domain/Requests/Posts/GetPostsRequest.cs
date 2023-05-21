using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.Posts;

public record GetPostsRequest(
    int Page,
    Guid UserId)
    : IRequest<PaginationResponse<PostResponse>>;
