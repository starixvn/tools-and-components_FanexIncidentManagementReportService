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
                var getIncidentWeeklyCreateNewReport = _incidentReportService.GetIncidentWeeklyCreateNewReport();
                var getIncidentWeeklyClosedReport = _incidentReportService.GetIncidentWeeklyClosedReport();
                var getIncidentWeeklyPendingReport = _incidentReportService.GetIncidentWeeklyPendingReport();
                var getIncidentWeeklyP1Report = _incidentReportService.GetIncidentWeeklyP1Report();
                var getRedmineUrl = _incidentReportService.GetRedmineUrl();
                var model = new IncidentReportViewModel()
                {
                    CreateNewIncidentReport = getIncidentWeeklyCreateNewReport,
                    ClosedIncidentReport = getIncidentWeeklyClosedReport,
                    PendingIncidentReport = getIncidentWeeklyPendingReport,
                    P1IncidentReport = getIncidentWeeklyP1Report,
                    RedmineUrl = getRedmineUrl + "/issues/"

                };

                var templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates/IncidentDailyReportTemplate.html");
                var emailTemplate = File.ReadAllText(templateFolderPath);

                var content = Engine.Razor.RunCompile(emailTemplate, "templateKey", typeof(IncidentReportViewModel), model);

                return content;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }            
        }
    }
}
