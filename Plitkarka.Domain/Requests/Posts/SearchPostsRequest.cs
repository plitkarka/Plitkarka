using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.Posts;

public record SearchPostsRequest(
    int Page,
    string Filter)
    : IRequest<PaginationResponse<PostResponse>>;
