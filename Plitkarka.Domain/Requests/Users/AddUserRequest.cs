using MediatR;
using Plitkarka.Domain.Models;

namespace Plitkarka.Domain.Requests.Users;

public record AddUserRequest(User NewUser)
    : IRequest<Guid>;