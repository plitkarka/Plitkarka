using MediatR;

namespace Plitkarka.Domain.Requests.Authentication;

public record LoginByIdRequest(Guid Id) 
    : IRequest<string>;
