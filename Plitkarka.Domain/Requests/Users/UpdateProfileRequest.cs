using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.Users;

public record class UpdateProfileRequest(
    string? Login,
    string? Name,
    string? Description,
    string? Link) 
    : IRequest<UserDataResponse>;
