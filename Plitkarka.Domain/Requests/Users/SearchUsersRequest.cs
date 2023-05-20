using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.Users;

public record SearchUsersRequest(
    int Page,
    string Filter)
    : IRequest<UsersListResponse>;
