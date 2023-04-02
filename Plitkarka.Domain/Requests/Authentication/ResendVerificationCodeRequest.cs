using MediatR;

namespace Plitkarka.Domain.Requests.Authentication;

public record ResendVerificationCodeRequest(string Email)
    : IRequest<string>;
