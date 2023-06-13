using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.ChatHub;

public record SendPostMessageRequest(
    Guid ReceiverId,
    Guid PostId)
    : IRequest<HubNotificationHandlerResponse>;