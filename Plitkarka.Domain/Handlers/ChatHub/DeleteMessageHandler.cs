using MediatR;
using Plitkarka.Domain.Requests.ChatHub;
using Plitkarka.Domain.ResponseModels;

namespace Plitkarka.Domain.Handlers.ChatHub;

public class DeleteMessageHandler : IRequestHandler<DeleteMessageRequest, HubNotificationHandlerResponse>
{
    public Task<HubNotificationHandlerResponse> Handle(DeleteMessageRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
