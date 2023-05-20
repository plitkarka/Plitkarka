using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.Users;

public record GetUserDataRequest(Guid UserId)
    : IRequest<UserDataResponse>;
