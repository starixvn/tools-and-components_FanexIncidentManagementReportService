namespace Fanex.IncidentManagementReport.Service.Job
{
    using Fanex.IncidentManagementReport.Service.Core;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Quartz;
    using System;

    public class IncidentDailyReportJob : JobBase
    {
        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var incidentManagementReportService = new IncidentManagementReportService();
            var body = incidentManagementReportService.IncidentDailyReportEmailTemplate();

            return new JobResultMessage
            {
                Mail = new Mail
                {
                    Subject = "[IMS] Daily Incident Report on " + Utilities.GetDisplayShortDate(DateTime.Now),
                    Title = "Auto Mail Daily Incident Report ",
                    Body = body
                },
                JobStatus = MailDelivery.Client.Enumerations.JobStatus.Success,
                Schedule = schedule
            };
        }
    }
}
