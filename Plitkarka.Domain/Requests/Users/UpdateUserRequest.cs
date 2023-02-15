using MediatR;
using Plitkarka.Domain.Models;

namespace Plitkarka.Domain.Requests.Users;

public record UpdateUserRequest(User ToUpdate)
    : IRequest<User>;