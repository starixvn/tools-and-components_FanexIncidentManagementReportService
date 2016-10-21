using System.ServiceProcess;
using Fanex.IncidentManagementReport.Service.Core;
using Fanex.MailDelivery.Client.Handlers;

namespace Fanex.IncidentManagementReport.Service
{
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
