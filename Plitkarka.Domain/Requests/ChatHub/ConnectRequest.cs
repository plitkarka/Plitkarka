using MediatR;

namespace Plitkarka.Domain.Requests.ChatHub;

public record ConnectRequest(string ConnectionId) 
    : IRequest;
