namespace Fanex.IncidentManagementReport.Services.Core.Services;

using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;

public interface ILogServiceProvider
{
    Task ErrorSentryAsync(Exception exception);
}

public class LogServiceProvider : ILogServiceProvider
{
    private readonly Logging.ILogger logger;

    public LogServiceProvider(Logging.ILogger logger)
    {
        this.logger = logger;
    }

    public async Task ErrorSentryAsync(Exception exception)
    {
        var logMessage = new LogMessage(exception.Message)
            .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
            .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
            .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
            .ToString();
        await logger.ErrorAsync(logMessage);
    }
}