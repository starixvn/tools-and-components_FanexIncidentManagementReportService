namespace Fanex.IncidentManagementReport.Services.Jobs
{
    using System;
    using Fanex.IncidentManagementReport.Services.Core.Extensions;
    using Fanex.IncidentManagementReport.Services.Services;
    using Fanex.MailDelivery.Client.Jobs;
    using Fanex.MailDelivery.Core.Models;
    using Quartz;

    public class IncidentWeeklyReportJob : JobBase
    {
        private readonly IIncidentReportManagementService incidentReportManagementService;

        public IncidentWeeklyReportJob(IIncidentReportManagementService incidentReportManagementService)
        {
            this.incidentReportManagementService = incidentReportManagementService;
        }

        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
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