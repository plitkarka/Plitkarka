using MediatR;
using Plitkarka.Domain.Models;

namespace Plitkarka.Domain.Requests.Users;

public record UpdateUserRequest(UserUpdate ToUpdate)
    : IRequest<User>;