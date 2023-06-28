namespace Plitkarka.Domain.ResponseModels;

public record HubNotification
{
    public string Type { get; set; }

    public Guid SenderId { get; set; }

    public Guid ObjectId { get; set; }

    public string SenderLogin { get; set; }

    public string Message { get; set; }
}
