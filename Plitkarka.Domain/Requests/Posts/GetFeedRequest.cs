using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.Posts;

public record GetFeedRequest(
    int Page) 
    : IRequest<PaginationResponse<PostResponse>>;
