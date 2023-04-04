using MediatR;
using Plitkarka.Domain.Models;

namespace Plitkarka.Domain.Requests.Authentication;

public record SignInRequest(
    string Email,
    string Password)
    : IRequest<TokenPair>;