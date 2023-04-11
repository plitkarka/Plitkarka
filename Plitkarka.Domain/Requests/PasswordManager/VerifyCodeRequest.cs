using MediatR;

namespace Plitkarka.Domain.Requests.PasswordManager;

public record VerifyCodeRequest(string Email, string PasswordCode)
    : IRequest<string>;
