namespace Fanex.IncidentManagementReport.Service.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.IncidentManagementReport.Service.Model;

    public class IncidentReportService : BaseService, IIncidentReportService
    {
        private readonly string projectIdIgnore = "31,56,65,66";
        private IIssuesService _issuesService;
        private IProjectService _projectService;

        public IncidentReportService() : this(null, null) 
        { 
        }

        public IncidentReportService(IIssuesService issuesService, IProjectService projectService)
        {
            _issuesService = issuesService ?? new IssuesService();
            _projectService = projectService ?? new ProjectService();
        }

        public IList<int> ProjectIdIgnore
        {
            get
            {
                var projectIds = projectIdIgnore.Split(',');
                return projectIds.Select(t => int.Parse(t)).ToList();
            }
        }

        public IList<IncidentReportModel> GetIncidentCreateNewReport(DateTime fromDate, DateTime toDate)
        {
            _issuesService.FromDate = fromDate;
            _issuesService.ToDate = toDate;

            var issues = _issuesService.CreateNewIssue;

            if (issues != null)
            {
                issues = issues.Where(t => !ProjectIdIgnore.Contains(t.Project.Id)).OrderByDescending(t => t.IncidentDate).ToList();
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
                issues = issues.Where(t => !ProjectIdIgnore.Contains(t.Project.Id)).OrderByDescending(t => t.IncidentDate).ToList();
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
            var query = issues.Select(iss => new IncidentReportModel
            {
                IncidentId = iss.Id,
                IncidentTitle = iss.Subject,
                CategoryName = (_projectService.GetParentProjectById(iss.Project.Id) != null
                    ? _projectService.GetParentProjectById(iss.Project.Id).Name + " - "
                    : string.Empty) + iss.Project.Name,
                Assignee = iss.Assignee != null ? iss.Assignee.Name : string.Empty,
                IncidentDate = iss.IncidentDate,
                Creator = iss.Creator.Name,
                Priority = isDaily ? iss.Priority.Name : GetPriorityName(iss.Priority.Id),
                PriorityId = iss.Priority.Id,
                Status = iss.Status.Name,
                UpdatedDate = iss.UpdatedDate,
                CreatedDate = iss.CreatedDate,
                ResolvedDate = iss.ResolvedDate
            });

            return query.ToList();
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
                issues = issues.Where(t => !ProjectIdIgnore.Contains(t.Project.Id)).OrderByDescending(t => t.IncidentDate).ToList();
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
                    .Where(t => !((t.CreatedDate - DateTime.Now.Date.AddDays(-1)).Days == 0 && t.Priority.Id == 1)).ToList();
                return GenerateIncidentModel(issues);
            }

            return new List<IncidentReportModel>();
        }

        public IList<IncidentReportModel> GetWeeklyIncidentPendingReport(DateTime toDate)
        {
            var issues = _issuesService.PendingIssue;
            if (issues != null)
            {
                issues = issues.Where(t => !ProjectIdIgnore.Contains(t.Project.Id)).OrderByDescending(t => t.IncidentDate).ToList();
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
