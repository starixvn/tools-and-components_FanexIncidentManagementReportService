namespace Fanex.IncidentManagementReport.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using System.Threading.Tasks;
    using Topshelf;

    public static class Program
    {
        public static void Main()
        {
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[]
            {
                new Service()
            };
            ServiceBase.Run(servicesToRun);

            ////var incidentManagementReportService = new IncidentManagementReportService();
            ////var body = incidentManagementReportService.incidentReportEmailTemplate();
            ////Console.WriteLine(body);
            ////Console.ReadKey();
        }
    }
}
