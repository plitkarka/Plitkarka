using MediatR;

namespace Plitkarka.Domain.Requests.PasswordManager;

public record SendEmailRequest(string Email)
    : IRequest<string>;
