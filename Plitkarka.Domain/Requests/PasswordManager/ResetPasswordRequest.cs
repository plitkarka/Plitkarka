using MediatR;
using Plitkarka.Domain.Models;

namespace Plitkarka.Domain.Requests.PasswordManager;

public record ResetPasswordRequest (string Email,string PasswordCode,string Password)
    : IRequest<TokenPair>;