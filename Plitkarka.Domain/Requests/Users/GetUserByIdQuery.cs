using MediatR;
using Plitkarka.Domain.Models;

namespace Plitkarka.Domain.Requests.Users;

public record GetUserByIdQuery (Guid Id) 
    : IRequest<User?>;