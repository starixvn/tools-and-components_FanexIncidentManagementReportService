using System;
using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Client.Messages;
using Fanex.MailDelivery.Client.Models;
using Quartz;

namespace Fanex.IncidentManagementReport.Service.Job
{
    class IncidentWeeklyReportJob : JobBase
    {
        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var incidentManagementReportService = new IncidentManagementReportService();
            var body = incidentManagementReportService.IncidentWeeklyReportEmailTemplate();

            return new JobResultMessage
            {
                Mail = new Mail
                {
                    Subject = "Weekly Incident Report on " + DateTime.Now.AddDays(-1).ToShortDateString(),
                    Title = "Auto Mail Weekly Incident Report ",
                    Body = body
                },
                JobStatus = MailDelivery.Client.Enumerations.JobStatus.Success,
                Schedule = schedule
            };
        }
    }
}
