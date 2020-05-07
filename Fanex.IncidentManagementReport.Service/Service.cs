namespace Fanex.IncidentManagementReport.Service
{
    using Fanex.IncidentManagementReport.Service.Core;
    using Fanex.Logging;
    using Fanex.Logging.Sentry;
    using Fanex.MailDelivery.Client;
    using Sentry;
    using System.Collections.Generic;
    using System.Configuration;
    using System.ServiceProcess;

    public partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();

            Utilities.RazorEngineConfig();

            LogManager
            .SetDefaultLogCategory(ConfigurationManager.AppSettings["LogCategory"])
            .Use(new List<ILogEngine>
            {
                new SentryLogging(new SentryEngineOptions
                {
                    Dsn = new Dsn(ConfigurationManager.AppSettings["SentryUrl"])
                })
            });
        }

        protected override void OnStart(string[] args)
        {
            MailClientHandler.Start();
        }

        protected override void OnStop()
        {
            MailClientHandler.Stop();
        }
    }
}
