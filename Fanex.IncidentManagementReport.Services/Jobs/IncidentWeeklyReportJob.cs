namespace Fanex.IncidentManagementReport.Services.Jobs
{
    using Core.Extensions;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Core.Models;
    using Quartz;
    using Services;
    using System;

    public class IncidentWeeklyReportJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var services = ServiceExtension.Services.BuildServiceProvider();

            IIncidentReportManagementService incidentReportManagementService = services.GetRequiredService<IIncidentReportManagementService>();

            var body = incidentReportManagementService.GenerateIncidentWeeklyReportContent().Result;
            var fromDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Monday);
            var toDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Sunday);

            var mail = new Mail
            {
                Subject = "[IMS] Weekly Incident Report from " + Utilities.GetDisplayShortDate(fromDate) + " to " + Utilities.GetDisplayShortDate(toDate),
                Title = "Auto Mail Weekly Incident Report ",
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