using Microsoft.Extensions.Logging;

namespace Plitkarka.Commons.Logger;

public static partial class AzureLoggerExtensions
{
    [LoggerMessage(
        EventId = 200,
        Level = LogLevel.Error,
        Message = "An error ocured while working with database: {message}")]
    public static partial void LogDatabaseError(
        this ILogger logger,
        string message);

    [LoggerMessage(
        EventId = 201,
        Level = LogLevel.Error,
        Message = "An error ocured while working with S3: {message}")]
    public static partial void LogS3Error(
        this ILogger logger,
        string message);
}
