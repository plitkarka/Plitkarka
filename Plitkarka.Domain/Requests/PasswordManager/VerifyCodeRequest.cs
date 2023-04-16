using MediatR;
using Plitkarka.Domain.Models;

namespace Plitkarka.Domain.Requests.PasswordManager;

public record VerifyCodeRequest(string Email, string PasswordCode)
    : IRequest<VerifyCodeResponse>;
