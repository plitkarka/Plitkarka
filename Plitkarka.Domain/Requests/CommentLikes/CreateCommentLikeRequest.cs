using MediatR;

namespace Plitkarka.Domain.Requests.CommentLikes;

public record CreateCommentLikeRequest(Guid CommentId)
    : IRequest<Guid>;