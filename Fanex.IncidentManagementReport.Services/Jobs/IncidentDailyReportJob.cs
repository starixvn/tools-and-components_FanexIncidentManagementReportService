namespace Fanex.IncidentManagementReport.Services.Jobs
{
    using System;
    using Fanex.IncidentManagementReport.Services.Core.Extensions;
    using Fanex.IncidentManagementReport.Services.Core.Services;
    using Fanex.IncidentManagementReport.Services.Services;
    using MailDelivery.Client.Jobs;
    using MailDelivery.Core.Models;
    using Quartz;

    public class IncidentDailyReportJob : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var services = ServiceExtension.Services.BuildServiceProvider();
            ILogServiceProvider logService = services.GetService<ILogServiceProvider>();
            logService.LogFile($"Daily Incident Report called at: {DateTime.Now}");

            IIncidentReportManagementService incidentReportManagementService = services.GetRequiredService<IIncidentReportManagementService>();
            var body = incidentReportManagementService.GenerateIncidentDailyReportContent().Result;

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