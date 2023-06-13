using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.ChatHub;

public record DeleteMessageRequest(
    Guid ReceiverId,
    Guid MessageId,
    bool ForAll)
    : IRequest<HubNotificationHandlerResponse>;