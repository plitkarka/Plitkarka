using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.Comments;

public record GetPostCommentsRequest(
    int Page,
    Guid PostId)
    : IRequest<PaginationResponse<CommentResponse>>;
