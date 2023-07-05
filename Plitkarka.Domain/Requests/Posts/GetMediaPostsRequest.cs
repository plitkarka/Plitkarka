using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.Posts;

public record GetMediaPostsRequest(
    int Page,
    Guid UserId)
    : IRequest<PaginationResponse<PostResponse>>;