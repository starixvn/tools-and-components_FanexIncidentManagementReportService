namespace Fanex.IncidentManagementReport.Service.Job
{
    using Core;
    using Logging;
    using MailDelivery.Client.Jobs;
    using MailDelivery.Core.Models;
    using Quartz;
    using System;

    public class IncidentDailyReportJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var jobMessage = new JobMessage
            {
                Mail = new Mail
                {
                    Title = "Auto Mail Daily Incident Report "
                },
                Schedule = schedule
            };

            try
            {
                jobMessage.Mail.Subject = "[IMS] Daily Incident Report on " + Utilities.GetDisplayShortDate(DateTime.Now);

                var incidentManagementReportService = new IncidentManagementReportService();
                var body = incidentManagementReportService.IncidentDailyReportEmailTemplate();

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
