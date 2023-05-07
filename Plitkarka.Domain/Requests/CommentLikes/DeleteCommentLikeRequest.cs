using MediatR;

namespace Plitkarka.Domain.Requests.CommentLikes;

public record DeleteCommentLikeRequest(Guid CommentId)
    : IRequest;