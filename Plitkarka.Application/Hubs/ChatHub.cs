using MediatR;
using Microsoft.AspNetCore.SignalR;
using Plitkarka.Domain.Filters;
using Plitkarka.Domain.Requests.ChatHub;
using Plitkarka.Domain.ResponseModels;
using SignalRSwaggerGen.Attributes;

namespace Plitkarka.Application.Hubs;

[SignalRHub(
    path: "/signalr/chat", 
    description: $@" 
        Each Connect and Disconnect method is called automatically using Microsoft.AspNetCore.SignalR.Client tools
        If initial connection to Hub is failed it will throw exception during the call of StartAsync method of Hub Connection on client side.
        To connect ChatHub client side must provide Access Token with the help of AccessTokenProvider when creating HubConnectionBuilder. 
        If any action request returns 401 Unauthorized status code client side must refresh tokens and provide new Access Token with AccessTokenProvider.
        Each action request returns 'SignalRResponse' object that contains two fields: 'Code' and 'Message'.
        'Code' - status code of operation.
        'Message' - additional information.
    ")]
public class ChatHub : Hub
{
    private IMediator _mediator { get; init; }

    public ChatHub(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [SignalRMethod(
        summary: "Connects user to hub",
        description: $@"
            Overrides basic hub connection to add ConnectionId to database.
        ")]
    [Authorize]
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        await _mediator.Send(new ConnectRequest(Context.ConnectionId));
    }


    [SignalRMethod(
        summary: "Allows to send simple message",
        description: $@"
            Allows user to send text message to other user. 
            If users had no messages sent before, creates chat with configurations for both users.
            Returns 400 if message is empty.
            Send 'MessageEvent' notification to all connections of message receiver.
        ")]
    [Authorize]
    public async Task<SignalRResponse> Message(
        Guid receiverId,
        string message)
    {
        var res = await _mediator.Send(
            new SendMessageRequest(receiverId, message));

        await Notify("MessageEvent", res);

        return SignalRResponse.ReturnOK("Message sent");
    }


    [SignalRMethod(
        summary: "Allows to send message with post",
        description: $@"
            Allows user to send posts to other user. 
            If users had no messages sent before, creates chat with configurations for both users.
            Allows text message to be sent as addition to the post.
            Returns 400 if post with given Id is not found.
            Send 'PostMessageEvent' notification to all connections of message receiver.
        ")]
    [Authorize]
    public async Task<SignalRResponse> PostMessage(
        Guid receiverId,
        Guid postId,
        string message)
    {
        var res = await _mediator.Send(
            new SendPostMessageRequest(receiverId, postId, message));

        await Notify("PostMessageEvent", res);

        return SignalRResponse.ReturnOK("Message sent");
    }


    [SignalRMethod(
        summary: "Allows to send message with image",
        description: $@"
            Allows user to send image to other user. 
            If users had no messages sent before, creates chat with configurations for both users.
            Allows text message to be sent as addition to the post.
            Returns 400 if image size is 0.
            Send 'ImageMessageEvent' notification to all connections of message receiver.
        ")]
    [Authorize]
    public async Task<SignalRResponse> ImageMessage(
        Guid receiverId,
        IFormFile image,
        string message)
    {
        var res = await _mediator.Send(
            new SendImageMessageRequest(receiverId, image, message));

        await Notify("ImageMessageEvent", res);

        return SignalRResponse.ReturnOK("Message sent");
    }


    [SignalRMethod(
        summary: "Allows user to delete his message",
        description: $@"
            Allows user to delete his own message.
            Returns 400 if message with give Id is not found or user tries to delete message made by other user.
            Send 'DeleteMessageEvent' notification to all connections of deleted message chat members.
        ")]
    [Authorize]
    public async Task<SignalRResponse> DeleteMessage(
        Guid messageId)
    {
        var res = await _mediator.Send(
            new DeleteMessageRequest(messageId));

        await Notify("DeleteMessageEvent", res);

        return SignalRResponse.ReturnOK("Message deleted");
    }


    [SignalRMethod(
        summary: "Disconnects user from the hub",
        description: $@"
            Overrides basic hub disconnect method to remove ConnectionId from database.
        ")]
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
