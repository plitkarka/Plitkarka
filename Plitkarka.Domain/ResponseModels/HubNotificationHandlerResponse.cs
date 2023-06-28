namespace Plitkarka.Domain.ResponseModels;

public record HubNotificationHandlerResponse
{
    public IList<string> ConnectionIds { get; set; }

    public HubNotification Notification { get; set; }
}
