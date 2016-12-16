namespace Fanex.IncidentManagementReport.Service
{
    using Fanex.IncidentManagementReport.Service.Core;
    using Fanex.MailDelivery.Client;
    using System.ServiceProcess;

    public partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();

            Utilities.RazorEngineConfig();
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
