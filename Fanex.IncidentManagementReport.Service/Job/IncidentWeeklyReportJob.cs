namespace Fanex.IncidentManagementReport.Service.Job
{
    using Fanex.IncidentManagementReport.Service.Core;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Core.Models;
    using Quartz;
    using System;

    public class IncidentWeeklyReportJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var incidentManagementReportService = new IncidentManagementReportService();
            var body = incidentManagementReportService.IncidentWeeklyReportEmailTemplate();

            var fromDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Monday);
            var toDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Sunday);

            return new JobMessage
            {
                Mail = new Mail
                {
                    Subject = "[IMS] Weekly Incident Report from " + Utilities.GetDisplayShortDate(fromDate) + " to " + Utilities.GetDisplayShortDate(toDate),
                    Title = "Auto Mail Weekly Incident Report ",
                    Body = body
                },
                JobStatus = JobStatus.Success,
                Schedule = schedule
            };
        }
    }
}
