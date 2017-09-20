namespace Fanex.IncidentManagementReport.Service.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Fanex.IncidentManagementReport.Service.Model;

    public class IncidentReportService : BaseService, IIncidentReportService
    {
        private readonly IIssuesService _issuesService;
        private readonly IProjectService _projectService;

        public IncidentReportService() : this(null, null)
        {
        }

        public IncidentReportService(IIssuesService issuesService, IProjectService projectService)
        {
            _issuesService = issuesService ?? new IssuesService();
            _projectService = projectService ?? new ProjectService();
            var projectIds = "31,56,65,66".Split(',');
            ProjectIdIgnore = projectIds.Select(t => int.Parse(t)).ToList();
        }

        public IList<int> ProjectIdIgnore { get; set; }

        public IList<IncidentReportModel> GetIncidentCreateNewReport(DateTime fromDate, DateTime toDate)
        {
            _issuesService.FromDate = fromDate;
            _issuesService.ToDate = toDate;

            var issues = _issuesService.CreateNewIssue;

            if (issues != null)
            {
                issues = issues.Where(t => !ProjectIdIgnore.Contains(t.Project.Id))
                    .OrderByDescending(t => t.IncidentDate).ToList();
                return GenerateIncidentModel(issues, isDaily: false);
            }

            return new List<IncidentReportModel>();
        }

        public IList<IncidentReportModel> GetIncidentClosedReport(DateTime fromDate, DateTime toDate)
        {
            _issuesService.FromDate = fromDate;
            _issuesService.ToDate = toDate;

            var issues = _issuesService.CloseIssue;
            if (issues != null)
            {
                issues = issues.Where(t => !ProjectIdIgnore.Contains(t.Project.Id))
                    .OrderByDescending(t => t.IncidentDate).ToList();
                return GenerateIncidentModel(issues, isDaily: false);
            }

            return new List<IncidentReportModel>();
        }

        public string GetRedmineUrl()
        {
            return RedmineUrl;
        }

        private IList<IncidentReportModel> GenerateIncidentModel(IList<IssuesViewModel> issues, bool isDaily = true)
        {
            IList<IncidentReportModel> result = new List<IncidentReportModel>();
            foreach (var iss in issues)
            {
                IncidentReportModel incident = new IncidentReportModel();

                incident.IncidentId = iss.Id;
                incident.IncidentTitle = iss.Subject;
                incident.CategoryName = (_projectService.GetParentProjectById(iss.Project.Id) != null
                                            ? _projectService.GetParentProjectById(iss.Project.Id).Name + " - "
                                            : string.Empty) + iss.Project.Name;
                incident.Assignee = iss.Assignee != null ? iss.Assignee.Name : string.Empty;

                incident.Creator = iss.Creator.Name;
                incident.Priority = isDaily ? iss.Priority.Name : GetPriorityName(iss.Priority.Id);
                incident.PriorityId = iss.Priority.Id;
                incident.Status = iss.Status.Name;
                incident.UpdatedDate = iss.UpdatedDate;

                incident.CreatedDate = iss.CreatedDate;
                incident.ClosedDate = iss.ClosedDate;

                // start_date
                incident.IncidentDate = ExtractDate(ReportedDateCustomFieldId, iss);
                // due_date
                incident.ResolvedDate = ExtractDate(ResolvedDateCustomFieldId, iss);

                result.Add(incident);
            }

            return result;
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
                // Valid time
                // Convert to UTC
                parsedDate = DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);

                // Minus the understanded timezone set in App.config
                parsedDate = parsedDate - TimeSpan.FromHours(CustomFieldTimeZone);
            }

            return parsedDate;
        }

        public IList<IncidentReportModel> GetDailyIncidentP1Report(DateTime fromDate, DateTime toDate)
        {
            _issuesService.FromDate = fromDate;
            _issuesService.ToDate = toDate;

            var issues = _issuesService.P1IssueDaily;
            if (issues != null)
            {
                issues = issues.Where(t => !ProjectIdIgnore.Contains(t.Project.Id)).ToList();
                return GenerateIncidentModel(issues);
            }

            return new List<IncidentReportModel>();
        }

        public IList<IncidentReportModel> GetWeeklyIncidentP1Report(DateTime fromDate, DateTime toDate)
        {
            _issuesService.FromDate = fromDate;
            _issuesService.ToDate = toDate;

            var issues = _issuesService.P1Issue;
            if (issues != null)
            {
                issues = issues.Where(t => !ProjectIdIgnore.Contains(t.Project.Id))
                    .OrderByDescending(t => t.IncidentDate).ToList();
                return GenerateIncidentModel(issues, isDaily: false);
            }

            return new List<IncidentReportModel>();
        }

        public IList<IncidentReportModel> GetDailyIncidentPendingReport()
        {
            var issues = _issuesService.PendingIssueDaily;
            if (issues != null)
            {
                issues = issues.Where(t => !ProjectIdIgnore.Contains(t.Project.Id))
                    .Where(t => !((t.CreatedDate - DateTime.Now.Date.AddDays(-1)).Days == 0 && t.Priority.Id == 1))
                    .ToList();
                return GenerateIncidentModel(issues);
            }

            return new List<IncidentReportModel>();
        }

        public IList<IncidentReportModel> GetWeeklyIncidentPendingReport(DateTime toDate)
        {
            var issues = _issuesService.PendingIssue;
            if (issues != null)
            {
                issues = issues.Where(t => !ProjectIdIgnore.Contains(t.Project.Id))
                    .OrderByDescending(t => t.IncidentDate).ToList();
                var incidentModel = GenerateIncidentModel(issues, isDaily: false);
                return incidentModel;
            }

            return new List<IncidentReportModel>();
        }

        private string GetPriorityName(int priorityId)
        {
            var priorityname = string.Empty;
            switch (priorityId)
            {
                case 1:
                    priorityname = "P1";
                    break;
                case 2:
                    priorityname = "P2";
                    break;
                case 3:
                    priorityname = "P3";
                    break;
            }

            return priorityname;
        }
    }
}