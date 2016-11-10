using System;
using System.IO;
using Fanex.IncidentManagementReport.Service.Core;
using Fanex.IncidentManagementReport.Service.Model;
using Fanex.IncidentManagementReport.Service.Services;
using FluentDateTime;
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
                var fromDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Monday);
                var toDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Sunday);

                return IncidentReportGeneration(fromDate, toDate, "EmailTemplates/IncidentWeeklyReportTemplate.html", true);
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

                var toDate = nowDate.PreviousDay();
                var fromDate = toDate;

                return IncidentReportGeneration(fromDate, toDate, "EmailTemplates/IncidentDailyReportTemplate.html");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string IncidentReportGeneration(DateTime fromDate, DateTime toDate, string emailTemplatePath, bool isWeekly = false)
        {
            var getIncidentCreateNewReport = _incidentReportService.GetIncidentCreateNewReport(fromDate, toDate);
            var getIncidentClosedReport = _incidentReportService.GetIncidentClosedReport(fromDate, toDate);
            var getIncidentPendingReport = isWeekly ? _incidentReportService.GetWeeklyIncidentPendingReport(toDate) : _incidentReportService.GetIncidentPendingReport();
            var getIncidentP1Report = _incidentReportService.GetIncidentP1Report(fromDate, toDate);
            var getRedmineUrl = _incidentReportService.GetRedmineUrl();
            var model = new IncidentReportViewModel()
            {
                CreateNewIncidentReport = getIncidentCreateNewReport,
                ClosedIncidentReport = getIncidentClosedReport,
                PendingIncidentReport = getIncidentPendingReport,
                P1IncidentReport = getIncidentP1Report,
                RedmineUrl = getRedmineUrl + "/issues/",
                FromDate = fromDate,
                ToDate = toDate
            };

            var templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, emailTemplatePath);
            var emailTemplate = File.ReadAllText(templateFolderPath);

            var content = Engine.Razor.RunCompile(emailTemplate, Path.GetFileName(emailTemplatePath) + "v6", typeof(IncidentReportViewModel), model);

            return content;
        }
    }
}
