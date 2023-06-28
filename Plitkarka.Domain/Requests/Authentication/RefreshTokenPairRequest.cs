using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.Authentication;

public record RefreshTokenPairRequest (
    string RefreshToken,
    string UniqueIdentifier) 
    : IRequest <TokenPairResponse>;
