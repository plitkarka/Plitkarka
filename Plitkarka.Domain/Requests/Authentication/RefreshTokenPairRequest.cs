using MediatR;
using Plitkarka.Domain.Models;

namespace Plitkarka.Domain.Requests.Authentication;

public record RefreshTokenPairRequest (string RefreshToken) 
    : IRequest <TokenPair>;
