using Microsoft.Extensions.Logging;

namespace Plitkarka.Commons.Logger;

public static partial class CommonLoggerExtensions
{
    [LoggerMessage(
        EventId = 100,
        Level = LogLevel.Information,
        Message = "{message}")]
    public static partial void LogInfo(
        this ILogger logger,
        string message);

    [LoggerMessage(
        EventId = 101,
        Level = LogLevel.Error,
        Message = "{methodName}: argument '{argumentName}' is null")]
    public static partial void LogArgumentNullError(
        this ILogger logger,
        string methodName,
        string argumentName);
}
