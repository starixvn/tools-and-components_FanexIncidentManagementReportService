using System;
using Fanex.IncidentManagementReport.Service.Core;
using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Client.Messages;
using Fanex.MailDelivery.Client.Models;
using FluentDateTime;
using Quartz;

namespace Fanex.IncidentManagementReport.Service.Job
{
    class IncidentWeeklyReportJob : JobBase
    {
        protected override JobResultMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var incidentManagementReportService = new IncidentManagementReportService();
            var body = incidentManagementReportService.IncidentWeeklyReportEmailTemplate();

            var fromDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Monday);
            var toDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Sunday);

            return new JobResultMessage
            {
                Mail = new Mail
                {
                    Subject = "[IMS] Weekly Incident Report from " + fromDate.ToShortDateString() + " to " + toDate.ToShortDateString(),
                    Title = "Auto Mail Weekly Incident Report ",
                    Body = body
                },
                JobStatus = MailDelivery.Client.Enumerations.JobStatus.Success,
                Schedule = schedule
            };
        }
    }
}
