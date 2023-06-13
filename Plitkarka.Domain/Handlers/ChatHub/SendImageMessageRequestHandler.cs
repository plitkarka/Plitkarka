using MediatR;
using Plitkarka.Domain.Requests.ChatHub;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Handlers.ChatHub;

public class SendImageMessageRequestHandler : IRequestHandler<SendImageMessageRequest, HubNotificationHandlerResponse>
{
    public Task<HubNotificationHandlerResponse> Handle(SendImageMessageRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
