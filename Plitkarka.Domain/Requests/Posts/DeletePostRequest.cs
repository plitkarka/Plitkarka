using MediatR;

namespace Plitkarka.Domain.Requests.Posts;

public record DeletePostRequest (Guid id) 
    : IRequest;