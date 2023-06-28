using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.ChatHub;

public record SendMessageRequest(
    Guid ReceiverId,
    string Message)
    : IRequest<HubNotificationHandlerResponse>;
