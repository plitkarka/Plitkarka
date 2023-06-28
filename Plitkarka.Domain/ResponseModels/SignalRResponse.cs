using System.Net;

namespace Plitkarka.Domain.ResponseModels;

public record SignalRResponse
{
    public int Code { get; set; }

    public string Message { get; set; }

    public static SignalRResponse ReturnOK(string? message = null) =>
        new SignalRResponse
        {
            Code = (int)HttpStatusCode.OK,
            Message = message ?? ""
        };
}
