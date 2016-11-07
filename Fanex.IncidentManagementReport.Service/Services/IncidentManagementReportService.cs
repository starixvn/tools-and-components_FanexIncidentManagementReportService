using System;
using System.IO;
using Fanex.IncidentManagementReport.Service.Model;
using Fanex.IncidentManagementReport.Service.Services;
using RazorEngine;
using RazorEngine.Templating;

namespace Fanex.IncidentManagementReport.Service
{
    public class IncidentManagementReportService
    {
        private IIncidentReportService _incidentReportService;
        public IncidentManagementReportService()
        {
            _incidentReportService = new IncidentReportService();
        }

        public string IncidentWeeklyReportEmailTemplate()
        {
            try
            {
                var nowDate = DateTime.Now;

                var fromDate = nowDate.AddDays(-1);
                var toDate = nowDate.AddDays(-8);


                return IncidentReportGeneration(fromDate, toDate, "EmailTemplates/IncidentWeeklyReportTemplate.html");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string IncidentDailyReportEmailTemplate()
        {
            try
            {
                var nowDate = DateTime.Now;

                var fromDate = nowDate.AddDays(-1);
                var toDate = nowDate.AddDays(-1);

                return IncidentReportGeneration(fromDate, toDate, "EmailTemplates/IncidentDailyReportTemplate.html");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string IncidentReportGeneration(DateTime fromDate, DateTime toDate, string emailTemplatePath)
        {
            var getIncidentWeeklyCreateNewReport = _incidentReportService.GetIncidentCreateNewReport(fromDate, toDate);
            var getIncidentWeeklyClosedReport = _incidentReportService.GetIncidentClosedReport(fromDate, toDate);
            var getIncidentWeeklyPendingReport = _incidentReportService.GetIncidentPendingReport(fromDate, toDate);
            var getIncidentWeeklyP1Report = _incidentReportService.GetIncidentP1Report(fromDate, toDate);
            var getRedmineUrl = _incidentReportService.GetRedmineUrl();
            var model = new IncidentReportViewModel()
            {
                CreateNewIncidentReport = getIncidentWeeklyCreateNewReport,
                ClosedIncidentReport = getIncidentWeeklyClosedReport,
                PendingIncidentReport = getIncidentWeeklyPendingReport,
                P1IncidentReport = getIncidentWeeklyP1Report,
                RedmineUrl = getRedmineUrl + "/issues/"

            };

            var templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, emailTemplatePath);
            var emailTemplate = File.ReadAllText(templateFolderPath);

            var content = Engine.Razor.RunCompile(emailTemplate, Path.GetFileName(emailTemplatePath), typeof(IncidentReportViewModel), model);

            return content;
        }
    }
}
