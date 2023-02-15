using MediatR;
using Plitkarka.Domain.Models;

namespace Plitkarka.Domain.Requests.Users;

public record DeleteUserByIdRequest(Guid Id)
    : IRequest;