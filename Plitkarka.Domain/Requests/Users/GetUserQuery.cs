using MediatR;
using Plitkarka.Domain.Models;
using Plitkarka.Infrastructure.Models;
using System.Linq.Expressions;

namespace Plitkarka.Domain.Requests.Users;

public record GetUserQuery(
    Expression<Func<UserEntity, bool>> Predicate)
    : IRequest<User?>;