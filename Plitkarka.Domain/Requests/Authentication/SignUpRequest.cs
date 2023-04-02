using MediatR;

namespace Plitkarka.Domain.Requests.Authentication;

public record SignUpRequest(
    string Login,
    string Name,
    string Email,
    string Password,
    DateTime BirthDate)
    : IRequest<string>;
