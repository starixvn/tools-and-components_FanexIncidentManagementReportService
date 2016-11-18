namespace Fanex.IncidentManagementReport.Service
{
    using Fanex.IncidentManagementReport.Service.Core;
    using Fanex.IncidentManagementReport.Service.Model;
    using Fanex.IncidentManagementReport.Service.Services;
    using FluentDateTime;
    using RazorEngine;
    using RazorEngine.Templating;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class IncidentManagementReportService
    {
        private IIncidentReportService incidentReportService;

        public IncidentManagementReportService() : this(null)
        {
        }

        public IncidentManagementReportService(IIncidentReportService incidentReportService)
        {
            this.incidentReportService = incidentReportService ?? new IncidentReportService();
        }

        public string IncidentWeeklyReportEmailTemplate()
        {
            try
            {
                var fromDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Monday);
                var toDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Sunday);

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
                var yesterday = DateTime.Now.PreviousDay();
                return IncidentDailyReportGeneration(yesterday, yesterday, "EmailTemplates/IncidentDailyReportTemplate.html");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string IncidentDailyReportGeneration(DateTime fromDate, DateTime toDate, string emailTemplatePath, bool isWeekly = false)
        {
            var getRedmineUrl = incidentReportService.GetRedmineUrl();
            var model = new IncidentDailyReportViewModel() 
            {
                FromDate = fromDate,
                ToDate = toDate,
                RedmineUrl = getRedmineUrl + "/issues/",
                PendingIncidents = GetPendingIncidentCategory(fromDate, toDate),
                P1Incidents = GetP1IncidentCategory(fromDate, toDate)
            };

            var templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, emailTemplatePath);
            var emailTemplate = File.ReadAllText(templateFolderPath);
            var content = Engine.Razor.RunCompile(emailTemplate, Path.GetFileName(emailTemplatePath) + "v6", typeof(IncidentDailyReportViewModel), model);

            return content;
        }

        private IList<IncidentGroup> GetP1IncidentCategory(DateTime fromDate, DateTime toDate)
        {
            var priority1Incidents = incidentReportService.GetDailyIncidentP1Report(fromDate, toDate);
            IList<IncidentReportModel> priority1GroupByCategory = priority1Incidents.GroupBy(t => t.CategoryName).Select(grp => grp.First()).ToList();
            IList<IncidentGroup> priority1IncidentGroups = priority1GroupByCategory.Select(t => new IncidentGroup()
            {
                CategoryName = t.CategoryName,
                Incidents = priority1Incidents.Where(x => x.CategoryName == t.CategoryName).ToList()
            }).ToList();

            return priority1IncidentGroups;
        }

        private IList<IncidentGroup> GetPendingIncidentCategory(DateTime fromDate, DateTime toDate)
        {
            var pendingIncidents = incidentReportService.GetDailyIncidentPendingReport();
            IList<IncidentReportModel> pendingIncidentGroupByCategory = pendingIncidents.GroupBy(t => t.CategoryName).Select(grp => grp.First()).ToList();
            IList<IncidentGroup> pendingIncidentGroups = pendingIncidentGroupByCategory.Select(t => new IncidentGroup()
            {
                CategoryName = t.CategoryName,
                Incidents = pendingIncidents.Where(x => x.CategoryName == t.CategoryName).ToList()
            }).ToList();

            return pendingIncidentGroups;
        }

        private string IncidentReportGeneration(DateTime fromDate, DateTime toDate, string emailTemplatePath)
        {
            var getIncidentCreateNewReport = incidentReportService.GetIncidentCreateNewReport(fromDate, toDate);
            var getIncidentClosedReport = incidentReportService.GetIncidentClosedReport(fromDate, toDate);
            var getIncidentPendingReport = incidentReportService.GetWeeklyIncidentPendingReport(toDate);
            var getIncidentP1Report = incidentReportService.GetWeeklyIncidentP1Report(fromDate, toDate);
            var getRedmineUrl = incidentReportService.GetRedmineUrl();
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
