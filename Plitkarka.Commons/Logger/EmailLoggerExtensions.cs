using Microsoft.Extensions.Logging;
using System.Reflection.Emit;

namespace Plitkarka.Commons.Logger;

public static partial class EmailLoggerExtensions
{
    [LoggerMessage(
        EventId = 300,
        Level = LogLevel.Error,
        Message = "Something went wrong sending an email: {message}")]
    public static partial void LogEmailSendingError(
        this ILogger logger,
        string message);
}
