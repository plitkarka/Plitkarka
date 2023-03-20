using MediatR;
using Plitkarka.Domain.Models;

namespace Plitkarka.Domain.Requests.Authentication;

public record LoginByIdRequest(Guid Id) 
    : IRequest<TokenPair>;
