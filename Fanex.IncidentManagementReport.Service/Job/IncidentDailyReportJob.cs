namespace Fanex.IncidentManagementReport.Service.Job
{
    using Fanex.IncidentManagementReport.Service.Core;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Core.Models;
    using Quartz;
    using System;

    public class IncidentDailyReportJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var incidentManagementReportService = new IncidentManagementReportService();
            var body = incidentManagementReportService.IncidentDailyReportEmailTemplate();

            return new JobMessage
            {
                Mail = new Mail
                {
                    Subject = "[IMS] Daily Incident Report on " + Utilities.GetDisplayShortDate(DateTime.Now),
                    Title = "Auto Mail Daily Incident Report ",
                    Body = body
                },
                JobStatus = JobStatus.Success,
                Schedule = schedule
            };
        }
    }
}
