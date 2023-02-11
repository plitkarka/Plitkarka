using Microsoft.Extensions.Logging;

namespace Plitkarka.Commons.Logger;

public static partial class MySqlLoggerExtensions
{
    [LoggerMessage(
        EventId = 200,
        Level = LogLevel.Error,
        Message = "{methodName}: an error ocured while working with database. Message: {message}")]
    public static partial void LogDatabaseError(
        this ILogger logger,
        string methodName,
        string message);
}
