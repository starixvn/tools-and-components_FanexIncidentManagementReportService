using System;
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
            var body = incidentManagementReportService.incidentReportEmailTemplate();

            return new JobResultMessage {
                Mail = new Mail
                {
                    Subject = "Daily Incident Report on " + DateTime.Now.ToShortDateString(),
                    Title = "Auto mail Daily Incident Report ",
                    Body = body
                },
                JobStatus = MailDelivery.Client.Enumerations.JobStatus.Success,
                Schedule = schedule
            };
        }
    }
}
