using MediatR;

namespace Plitkarka.Domain.Requests.ChatHub;

public record DisconnectRequest(string ConnectionId)
    : IRequest;