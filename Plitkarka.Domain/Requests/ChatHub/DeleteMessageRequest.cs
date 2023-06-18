using MediatR;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Requests.ChatHub;

public record DeleteMessageRequest(
    Guid MessageId)
    : IRequest<HubNotificationHandlerResponse>;