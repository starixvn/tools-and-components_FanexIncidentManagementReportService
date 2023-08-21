namespace Fanex.IncidentManagementReport.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Fanex.IncidentManagementReport.Services.Core.Enums;
    using Fanex.IncidentManagementReport.Services.Core.Models;
    using Fanex.IncidentManagementReport.Services.Core.Services.Interfaces;
    using Fanex.IncidentManagementReport.Services.Model;
    using Fanex.IncidentManagementReport.Services.Services.Interfaces;

    public class IncidentService : IIncidentService
    {
        private readonly IIssueService issueService;
        private readonly IProjectService projectService;
        private readonly AppSettings appSettings;
        private const TrackerEnum Tracker = TrackerEnum.Incident;

        public IncidentService(IIssueService issueService, IProjectService projectService, AppSettings appSettings)
        {
            this.issueService = issueService;
            this.projectService = projectService;
            this.appSettings = appSettings;
        }

        public async Task<IList<IncidentReportModel>> GetP1IncidentDailyAsync(DateTime fromDate, DateTime toDate)
        {
            var issues = await issueService.GetP1IssuesDailyAsync(fromDate, toDate, Tracker);
            if (issues is not null)
            {
                issues = issues.Where(t => !appSettings.IgnoredProjectIds.Contains(t.Project.Id)).ToList();
                return await GenerateIncidentModel(issues);
            }

            return new List<IncidentReportModel>();
        }

        public async Task<IList<IncidentReportModel>> GetPendingIncidentDailyAsync()
        {
            var issues = await issueService.GetPendingIssuesDailyAsync(Tracker);
            if (issues is not null)
            {
                issues = issues.Where(t => !appSettings.IgnoredProjectIds.Contains(t.Project.Id))
                    .Where(t => !((t.CreatedDate - DateTime.Now.Date.AddDays(-1)).Days == 0 && t.Priority.Id == 1))
                    .ToList();
                return await GenerateIncidentModel(issues);
            }

            return new List<IncidentReportModel>();
        }

        public async Task<IList<IncidentReportModel>> GetClosedIncidentAsync(DateTime fromDate, DateTime toDate)
        {
            var issues = await issueService.GetClosedIssuesAsync(fromDate, toDate, Tracker);
            if (issues is not null)
            {
                issues = issues.Where(t => !appSettings.IgnoredProjectIds.Contains(t.Project.Id))
                    .OrderByDescending(t => t.IncidentDate).ToList();
                return await GenerateIncidentModel(issues, isDaily: false);
            }

            return new List<IncidentReportModel>();
        }

        public async Task<IList<IncidentReportModel>> GetCreateNewIncidentAsync(DateTime fromDate, DateTime toDate)
        {
            var issues = await issueService.GetCreateNewIssuesAsync(fromDate, toDate, Tracker);
            if (issues != null)
            {
                issues = issues.Where(t => !appSettings.IgnoredProjectIds.Contains(t.Project.Id))
                    .OrderByDescending(t => t.IncidentDate).ToList();
                return await GenerateIncidentModel(issues, isDaily: false);
            }

            return new List<IncidentReportModel>();
        }

        public async Task<IList<IncidentReportModel>> GetP1IncidentWeeklyAsync(DateTime fromDate, DateTime toDate)
        {
            var issues = await issueService.GetP1IssuesAsync(fromDate, toDate, Tracker);
            if (issues is not null)
            {
                issues = issues.Where(t => !appSettings.IgnoredProjectIds.Contains(t.Project.Id))
                    .OrderByDescending(t => t.IncidentDate).ToList();
                return await GenerateIncidentModel(issues, isDaily: false);
            }

            return new List<IncidentReportModel>();
        }

        public async Task<IList<IncidentReportModel>> GetPendingIncidentWeeklyAsync(DateTime toDate)
        {
            var issues = await issueService.GetPendingIssuesAsync(toDate, Tracker);
            if (issues != null)
            {
                issues = issues.Where(t => !appSettings.IgnoredProjectIds.Contains(t.Project.Id))
                    .OrderByDescending(t => t.IncidentDate).ToList();
                var incidentModel = await GenerateIncidentModel(issues, isDaily: false);
                return incidentModel;
            }

            return new List<IncidentReportModel>();
        }

        private async Task<IList<IncidentReportModel>> GenerateIncidentModel(IList<IssuesViewModel> issues, bool isDaily = true)
        {
            IList<IncidentReportModel> result = new List<IncidentReportModel>();
            foreach (var iss in issues)
            {
                var parentProject = await projectService.GetParentProjectByIdAsync(iss.Project.Id);

                IncidentReportModel incident = new()
                {
                    IncidentId = iss.Id,
                    IncidentTitle = iss.Subject,
                    CategoryName = (parentProject is not null ? parentProject.Name + " - " : string.Empty) + iss.Project.Name,
                    Assignee = iss.Assignee != null ? iss.Assignee.Name : string.Empty,
                    Creator = iss.Creator.Name,
                    Priority = isDaily ? iss.Priority.Name : GetPriorityName(iss.Priority.Name),
                    PriorityId = iss.Priority.Id,
                    Status = iss.Status.Name,
                    UpdatedDate = iss.UpdatedDate,

                    CreatedDate = iss.CreatedDate,
                    ClosedDate = iss.ClosedDate,

                    // start_date
                    IncidentDate = ExtractDate(appSettings.ReportedDateCustomFieldId, iss),
                    // due_date
                    ResolvedDate = ExtractDate(appSettings.ResolvedDateCustomFieldId, iss)
                };

                result.Add(incident);
            }

            return result;
        }

        private static string GetPriorityName(string priorityName)
        {
            var value = string.Empty;
            if (!string.IsNullOrWhiteSpace(priorityName) && priorityName.Contains('-'))
            {
                value = "P" + priorityName.Split('-').FirstOrDefault().Trim();
            }
            return value;
        }

        private DateTime ExtractDate(int customFieldValue, IssuesViewModel model)
        {
            var dateTimeString = model.CustomFields.Where(x => x.Id == customFieldValue).Select(x => x.Value)
                .FirstOrDefault()?.ToString();
            var parsedDate = !string.IsNullOrEmpty(dateTimeString)
                ? DateTime.ParseExact(dateTimeString, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
                : new DateTime(1, 1, 1);
            if (parsedDate.Year > 1)
            {
                parsedDate = DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);

                parsedDate -= TimeSpan.FromHours(appSettings.CustomFieldTimeZone);
            }

            return parsedDate;
        }
    }
}