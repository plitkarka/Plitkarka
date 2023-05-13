using MediatR;

namespace Plitkarka.Domain.Requests.Comments;

public record DeleteCommentRequest(Guid CommentId)
    : IRequest;
