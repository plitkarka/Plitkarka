using MediatR;
using Microsoft.AspNetCore.SignalR;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Requests.ChatHub;
using Plitkarka.Domain.ResponseModels;
using SignalRSwaggerGen.Attributes;

namespace Plitkarka.Application.Hubs;

[SignalRHub("/signalr/chat")]
public class ChatHub : Hub
{
    private IMediator _mediator { get; init; }

    public ChatHub(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        await _mediator.Send(new ConnectRequest(Context.ConnectionId));
    }

    [Authorize]
    public async Task Message(Guid receiverId, string message)
    {
        var res = await _mediator.Send(
            new SendMessageRequest(receiverId, message));

        await Notify("MessageEvent", res);
    }

    [Authorize]
    public async Task PostMessage(Guid receiverId, Guid postId)
    {
        var res = await _mediator.Send(
            new SendPostMessageRequest(receiverId, postId));

        await Notify("PostMessageEvent", res);
    }

    [Authorize]
    public async Task ImageMessage(Guid receiverId, IFormFile image)
    {
        var res = await _mediator.Send(
            new SendImageMessageRequest(receiverId, image));

        await Notify("ImageMessageEvent", res);
    }

    [Authorize]
    public async Task DeleteMessage(Guid receiverId, Guid postId, bool forAll)
    {
        var res = await _mediator.Send(
            new DeleteMessageRequest(receiverId, postId, forAll));

        await Notify("DeleteMessageEvent", res);
    }

    [Authorize]
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _mediator.Send(new DisconnectRequest(Context.ConnectionId));  

        await base.OnDisconnectedAsync(exception);
    }

    private async Task Notify(string notificationType, HubNotificationHandlerResponse response)
    {
        foreach(var connectionId in response.ConnectionIds)
        {
            Console.WriteLine("Sending message " + notificationType + " to connection Id " + connectionId);
            await Clients.Client(connectionId).SendAsync(notificationType, response.Notification);
        }
    }
}
