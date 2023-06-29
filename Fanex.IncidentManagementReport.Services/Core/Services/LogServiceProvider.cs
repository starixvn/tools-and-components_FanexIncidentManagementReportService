namespace Fanex.IncidentManagementReport.Services.Core.Services
{
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using NLog;

    public interface ILogServiceProvider
    {
        void LogFile(string message);

        Task ErrorSentryAsync(Exception exception);
    }

    public class LogServiceProvider : ILogServiceProvider
    {
        private readonly Logging.ILogger logger;
        private readonly Logger nlog = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

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

        public void LogFile(string message)
        {
            nlog.Error(message);
        }
    }
}