using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.Authentication;

public record VerifyEmailRequest (
    string Email,
    string EmailCode)
    : IRequest<TokenPair>;
