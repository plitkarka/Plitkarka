using MediatR;
using Microsoft.AspNetCore.Http;

namespace Plitkarka.Domain.Requests.Users;

public record SetUserImageRequest(IFormFile Image) 
    : IRequest<Guid>;
