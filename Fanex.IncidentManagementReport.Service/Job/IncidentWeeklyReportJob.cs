namespace Fanex.IncidentManagementReport.Service.Job
{
    using Fanex.IncidentManagementReport.Service.Core;
    using Fanex.Logging;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Core.Models;
    using Quartz;
    using System;

    public class IncidentWeeklyReportJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobMessage = new JobMessage()
            {
                Mail = new Mail()
                {
                    Title = "Auto Mail Weekly Incident Report "
                },
                Schedule = schedule
            };

            try
            {
                var fromDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Monday);
                var toDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Sunday);
                jobMessage.Mail.Subject = "[IMS] Weekly Incident Report from " + Utilities.GetDisplayShortDate(fromDate) + " to " + Utilities.GetDisplayShortDate(toDate);

                var incidentManagementReportService = new IncidentManagementReportService();
                var body = incidentManagementReportService.IncidentWeeklyReportEmailTemplate();

                jobMessage.Mail.Body = body;
                jobMessage.JobStatus = JobStatus.Success;
            }
            catch (Exception ex)
            {
                jobMessage.JobStatus = JobStatus.JobFailed;
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                LogService.Error(message);
            }

            return jobMessage;
        }
    }
}
