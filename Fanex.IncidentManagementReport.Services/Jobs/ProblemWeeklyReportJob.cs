namespace Fanex.IncidentManagementReport.Services.Jobs
{
    using Core.Extensions;
    using MailDelivery.Client.Jobs;
    using MailDelivery.Core.Models;
    using Quartz;
    using Services;
    using System;

    public class ProblemWeeklyReportJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var services = ServiceExtension.Services.BuildServiceProvider();

            IProblemManagementReportService problemManagementReportService = services.GetRequiredService<IProblemManagementReportService>();

            var body = problemManagementReportService.GenerateProblemWeeklyReportContent().GetAwaiter().GetResult();
            var fromDate = DateTime.Now.DateOfPreviousWeek(DayOfWeek.Monday);
            var toDate = DateTime.Now.DateOfPreviousWeek(DayOfWeek.Sunday);

            var mail = new Mail
            {
                Subject = "[IMS] Weekly Problem Report from " + Utilities.GetDisplayShortDate(fromDate) + " to " + Utilities.GetDisplayShortDate(toDate),
                Title = "Auto Mail Weekly Problem Report ",
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