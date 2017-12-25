namespace Fanex.IncidentManagementReport.Service.Job
{
    using System;
    using Core;
    using Logging;
    using MailDelivery.Client.Jobs;
    using MailDelivery.Core.Models;
    using Quartz;

    public class ProblemWeeklyReportJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobMessage = new JobMessage
            {
                Mail = new Mail
                {
                    Title = "Auto Mail Weekly Problem Report "
                },
                Schedule = schedule
            };

            try
            {
                var fromDate = DateTime.Now.DateOfPreviousWeek(DayOfWeek.Monday);
                var toDate = DateTime.Now.DateOfPreviousWeek(DayOfWeek.Sunday);

                jobMessage.Mail.Subject = "[IMS] Weekly Problem Report from " + Utilities.GetDisplayShortDate(fromDate) + " to " + Utilities.GetDisplayShortDate(toDate);

                var ProblemManagementReportService = new ProblemManagementReportService();
                var body = ProblemManagementReportService.ProblemWeeklyReportEmailTemplate();

                jobMessage.Mail.Body = body;
                jobMessage.JobStatus = JobStatus.Success;
            }
            catch (Exception ex)
            {
                jobMessage.JobStatus = JobStatus.JobFailed;
                var message = ex.InnerException?.Message ?? ex.Message;
                LogService.Error(message);
            }

            return jobMessage;
        }
    }
}