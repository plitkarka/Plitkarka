using MediatR;
using Microsoft.AspNetCore.Http;

namespace Plitkarka.Domain.Requests.Posts;

public record CreatePostRequest (string? TextContent, IFormFile? Image) 
    : IRequest<Guid>;
