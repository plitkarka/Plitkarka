using MediatR;
using Plitkarka.Domain.Models;

namespace Plitkarka.Domain.Requests.Users;

public record GetUserByIdRequest (Guid Id) 
    : IRequest<User?>;