using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Client.Messages;
using Fanex.MailDelivery.Client.Models;
using Quartz;

namespace Fanex.IncidentManagementReport.Service.Job
{
    class IncidentDailyReportJob : JobBase
    {
        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var incidentManagementReportService = new IncidentManagementReportService();
            var body = incidentManagementReportService.IncidentDailyReportEmailTemplate();

            return new JobResultMessage
            {
                Mail = new Mail
                {
                    Subject = "Daily Incident Report on " + DateTime.Now.AddDays(-1).ToShortDateString(),
                    Title = "Auto Mail Daily Incident Report ",
                    Body = body
                },
                JobStatus = MailDelivery.Client.Enumerations.JobStatus.Success,
                Schedule = schedule
            };
        }
    }
}
