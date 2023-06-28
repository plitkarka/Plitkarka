using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.PostLikes;

public record GetLikedPostsRequest(
    int Page,
    Guid UserId)
    : IRequest<PaginationResponse<PostResponse>>;
