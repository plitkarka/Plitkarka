using MediatR;

namespace Plitkarka.Domain.Requests.Posts;

public record CreatePostRequest (string TextContent) 
    : IRequest<Guid>;
