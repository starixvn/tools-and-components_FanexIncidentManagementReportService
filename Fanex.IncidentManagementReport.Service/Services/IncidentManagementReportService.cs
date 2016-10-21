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

        public string incidentReportEmailTemplate()
        {
            try
            {
                var getIncidentDailyCreateNewReport = _incidentReportService.GetIncidentDailyCreateNewReport();
                var getIncidentDailyUpdateReport = _incidentReportService.GetIncidentDailyUpdateReport();
                var getIncidentDailyPendingReport = _incidentReportService.GetIncidentDailyPendingReport();
                var getRedmineUrl = _incidentReportService.GetRedmineUrl();
                var model = new IncidentReportViewModel()
                {
                    CreateNewIncidentReport = getIncidentDailyCreateNewReport,
                    ChangedIncidentReport = getIncidentDailyUpdateReport,
                    PendingIncidentReport = getIncidentDailyPendingReport,
                    RedmineUrl = getRedmineUrl + "/issues/"

                };

                var templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates/IncidentDailyReportTemplate.html");
                var emailTemplate = File.ReadAllText(templateFolderPath);

                var content = Engine.Razor.RunCompile(emailTemplate, "key", typeof(IncidentReportViewModel), model);

                return content;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }            
        }
    }
}
