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
            var newIncidents = incidentReportService.GetIncidentCreateNewReport(fromDate, toDate);
            var closedIncidents = incidentReportService.GetIncidentClosedReport(fromDate, toDate);
            var pendingIncidents = incidentReportService.GetWeeklyIncidentPendingReport(toDate);
            var firstPriorityIncidents = incidentReportService.GetWeeklyIncidentP1Report(fromDate, toDate);

            var newIncidentSummary = new IncidentPrioritySummary(newIncidents);
            var closedIncidentSummary = new IncidentPrioritySummary(closedIncidents);
            var pendingIncidentSummary = new IncidentPrioritySummary(pendingIncidents);

            var agingIncidentsLastWeek = GetAgingIncidentsLastWeek(firstPriorityIncidents, newIncidents, closedIncidents, pendingIncidents, fromDate, toDate);
            var agingIncidentsLast2Week = GetAgingIncidentsLast2Week(fromDate.AddDays(-7), toDate.AddDays(-7));

            var getRedmineUrl = incidentReportService.GetRedmineUrl();
            var model = new IncidentReportViewModel()
            {
                CreateNewIncidentSummary = newIncidentSummary,
                ClosedIncidentSummary = closedIncidentSummary,
                PendingIncidentSummary = pendingIncidentSummary,
                CreateNewIncidentReport = RemoveP1Incidents(firstPriorityIncidents, newIncidents),
                ClosedIncidentReport = RemoveP1Incidents(firstPriorityIncidents, closedIncidents),
                PendingIncidentReport = RemoveP1Incidents(firstPriorityIncidents, pendingIncidents),
                P1IncidentReport = firstPriorityIncidents,
                AgingDataLastWeek = agingIncidentsLastWeek,
                AgingDataLast2Week = agingIncidentsLast2Week,
                RedmineUrl = getRedmineUrl + "/issues/",
                FromDate = fromDate,
                ToDate = toDate
            };

            var templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, emailTemplatePath);
            var emailTemplate = File.ReadAllText(templateFolderPath);

            var content = Engine.Razor.RunCompile(emailTemplate, Path.GetFileName(emailTemplatePath) + "v6", typeof(IncidentReportViewModel), model);

            return content;
        }

        private IList<IncidentReportModel> GetAgingIncidentsLastWeek(
            IList<IncidentReportModel> firstPriorityIncidents, 
            IList<IncidentReportModel> newIncidents, 
            IList<IncidentReportModel> closedIncidents, 
            IList<IncidentReportModel> pendingIncidents,
            DateTime fromDate,
            DateTime toDate)
        {
            var incidents = CombineIncidents(firstPriorityIncidents, newIncidents, closedIncidents, pendingIncidents);
            incidents = GetAgingIncidents(incidents, fromDate, toDate);
            return incidents;
        }

        private IList<IncidentReportModel> GetAgingIncidentsLast2Week(DateTime fromDate, DateTime toDate)
        {
            var newIncidents = incidentReportService.GetIncidentCreateNewReport(fromDate, toDate);
            var closedIncidents = incidentReportService.GetIncidentClosedReport(fromDate, toDate);
            var pendingIncidents = incidentReportService.GetWeeklyIncidentPendingReport(toDate);
            var firstPriorityIncidents = incidentReportService.GetWeeklyIncidentP1Report(fromDate, toDate);

            var incidents = CombineIncidents(firstPriorityIncidents, newIncidents, closedIncidents, pendingIncidents);
            incidents = GetAgingIncidents(incidents, fromDate, toDate);
            return incidents;
        }

        private IList<IncidentReportModel> RemoveP1Incidents(IList<IncidentReportModel> firstPriorityIncidents, IList<IncidentReportModel> incidents)
        {
            incidents = incidents.Where(t => !firstPriorityIncidents.Any(x => x.IncidentId == t.IncidentId)).ToList();
            return incidents;
        }

        private IList<IncidentReportModel> CombineIncidents(
            IList<IncidentReportModel> firstPriorityIncidents, 
            IList<IncidentReportModel> newIncidents, 
            IList<IncidentReportModel> closedIncidents, 
            IList<IncidentReportModel> pendingIncidents)
        {
            List<IncidentReportModel> incidents = new List<IncidentReportModel>();

            incidents.AddRange(firstPriorityIncidents);
            incidents.AddRange(newIncidents.Where(t => !incidents.Any(x => x.IncidentId == t.IncidentId)));
            incidents.AddRange(closedIncidents.Where(t => !incidents.Any(x => x.IncidentId == t.IncidentId)));
            incidents.AddRange(pendingIncidents.Where(t => !incidents.Any(x => x.IncidentId == t.IncidentId)));

            return incidents;
        }

        private IList<IncidentReportModel> GetAgingIncidents(IList<IncidentReportModel> incidents, DateTime fromDate, DateTime toDate)
        {
            incidents = incidents.Where(t =>
                (t.PriorityId == (int)Priorities.P1 && t.IncidentDate.AddDays(1) < GetResolvedDate(t.ResolvedDate, toDate))
                || (t.PriorityId == (int)Priorities.P2 && t.IncidentDate.AddDays(2) < GetResolvedDate(t.ResolvedDate, toDate))
                || (t.PriorityId == (int)Priorities.P3 && t.IncidentDate.AddDays(7) < GetResolvedDate(t.ResolvedDate, toDate))).ToList();

            // no count incident which was resolved on this week
            incidents = incidents.Where(t => t.ResolvedDate.Year == 1 || (t.ResolvedDate.AddDays(-7) > fromDate && t.ResolvedDate.AddDays(-7) > t.IncidentDate)).ToList();

            return incidents;
        }

        private DateTime GetResolvedDate(DateTime resolvedDate, DateTime toDate)
        {
            return resolvedDate.Year == 1 ? toDate : resolvedDate;
        }
    }
}
