namespace Plitkarka.Domain.ResponseModels;

public record UserPreviewSubscriptionResponse : UserPreviewResponse
{
    public bool IsSubscribed { get; set; }
}
