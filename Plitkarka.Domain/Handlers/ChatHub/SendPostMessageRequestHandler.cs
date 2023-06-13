using MediatR;
using Plitkarka.Domain.Requests.ChatHub;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Handlers.ChatHub;

public class SendPostMessageRequestHandler : IRequestHandler<SendPostMessageRequest, HubNotificationHandlerResponse>
{
    public Task<HubNotificationHandlerResponse> Handle(SendPostMessageRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
