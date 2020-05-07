namespace Fanex.IncidentManagementReport.Service.Job
{
    using Core;
    using MailDelivery.Client.Jobs;
    using MailDelivery.Core.Models;
    using Quartz;
    using System;

    public class IncidentDailyReportJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var incidentManagementReportService = new IncidentManagementReportService();
            var body = incidentManagementReportService.IncidentDailyReportEmailTemplate();

            var mail = new Mail
            {
                Subject = "[IMS] Daily Incident Report on " + Utilities.GetDisplayShortDate(DateTime.Now),
                Title = "Auto Mail Daily Incident Report ",
                Body = body
            };

            return new JobMessage
            {
                Mail = mail,
                Schedule = schedule,
                JobStatus = JobStatus.Success
            };
        }
    }
}
