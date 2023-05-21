using MediatR;

namespace Plitkarka.Domain.Requests.Users;

public record GetUserImageRequest(Guid UserId)
    : IRequest<string>;