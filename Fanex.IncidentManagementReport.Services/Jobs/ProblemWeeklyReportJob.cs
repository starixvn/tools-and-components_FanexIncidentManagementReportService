namespace Fanex.IncidentManagementReport.Services.Jobs
{
    using System;
    using MailDelivery.Client.Jobs;
    using MailDelivery.Core.Models;
    using Quartz;

    public class ProblemWeeklyReportJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var ProblemManagementReportService = new ProblemManagementReportService();
            var body = ProblemManagementReportService.ProblemWeeklyReportEmailTemplate();
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