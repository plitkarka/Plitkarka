namespace Plitkarka.Domain.ResponseModels;

public record SignalRResponse
{
    public int Code { get; set; }

    public string Message { get; set; }
}
