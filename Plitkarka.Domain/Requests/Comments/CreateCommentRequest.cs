using MediatR;

namespace Plitkarka.Domain.Requests.Comments;

public record CreateCommentRequest(Guid PostId, string TextContent)
    : IRequest<Guid>;
