namespace Fanex.IncidentManagementReport.Service.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Model;
    using Model.Enumerations;

    public class ProblemReportService : BaseService, IProblemReportService
    {
        private readonly IIssuesService _issuesService;
        private readonly IProjectService _projectService;

        public ProblemReportService() : this(null, null)
        {
        }

        public ProblemReportService(IIssuesService issuesService, IProjectService projectService)
        {
            _issuesService = issuesService ?? new IssuesService();
            _projectService = projectService ?? new ProjectService();

            _issuesService.Tracker = Tracker.Problem;

            var projectIds = "31,56,65,66".Split(',');
            ProjectIdIgnore = projectIds.Select(int.Parse).ToList();
        }

        public IList<int> ProjectIdIgnore { get; set; }

        public IList<ProblemReportModel> GetProblemCreateNewReport(DateTime fromDate, DateTime toDate)
        {
            _issuesService.FromDate = fromDate;
            _issuesService.ToDate = toDate;

            var issues = _issuesService.CreateNewIssue;

            if (issues != null)
            {
                issues = issues.Where(t => !ProjectIdIgnore.Contains(t.Project.Id))
                    .OrderByDescending(t => t.IncidentDate).ToList();
                return GenerateProblemModel(issues, isDaily: false);
            }

            return new List<ProblemReportModel>();
        }

        public IList<ProblemReportModel> GetProblemClosedReport(DateTime fromDate, DateTime toDate)
        {
            _issuesService.FromDate = fromDate;
            _issuesService.ToDate = toDate;

            var issues = _issuesService.CloseIssue;
            if (issues != null)
            {
                issues = issues.Where(t => !ProjectIdIgnore.Contains(t.Project.Id))
                    .OrderByDescending(t => t.IncidentDate).ToList();
                return GenerateProblemModel(issues, isDaily: false);
            }

            return new List<ProblemReportModel>();
        }

        public string GetRedmineUrl()
        {
            return RedmineUrl;
        }

        private IList<ProblemReportModel> GenerateProblemModel(IList<IssuesViewModel> issues, bool isDaily = true)
        {
            IList<ProblemReportModel> result = new List<ProblemReportModel>();
            foreach (var iss in issues)
            {
                ProblemReportModel problem = new ProblemReportModel();

                problem.ProblemId = iss.Id;
                problem.ProblemTitle = iss.Subject;
                problem.CategoryName = (_projectService.GetParentProjectById(iss.Project.Id) != null
                                            ? _projectService.GetParentProjectById(iss.Project.Id).Name + " - "
                                            : string.Empty) + iss.Project.Name;
                problem.Assignee = iss.Assignee != null ? iss.Assignee.Name : string.Empty;

                problem.Creator = iss.Creator.Name;
                problem.Priority = isDaily ? iss.Priority.Name : GetPriorityName(iss.Priority.Id);
                problem.PriorityId = iss.Priority.Id;
                problem.Status = iss.Status.Name;
                ////problem.UpdatedDate = iss.UpdatedDate;

                problem.ReportedDate = iss.CreatedDate;
                problem.ClosedDate = iss.ClosedDate;

                // start_date
                problem.ReportedDate = ExtractDate(ReportedDateCustomFieldId, iss);
                // due_date
                problem.ResolvedDate = ExtractDate(ResolvedDateCustomFieldId, iss);

                result.Add(problem);
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

        public IList<ProblemReportModel> GetDailyProblemP1Report(DateTime fromDate, DateTime toDate)
        {
            _issuesService.FromDate = fromDate;
            _issuesService.ToDate = toDate;

            var issues = _issuesService.P1IssueDaily;
            if (issues != null)
            {
                issues = issues.Where(t => !ProjectIdIgnore.Contains(t.Project.Id)).ToList();
                return GenerateProblemModel(issues);
            }

            return new List<ProblemReportModel>();
        }

        public IList<ProblemReportModel> GetWeeklyProblemP1Report(DateTime fromDate, DateTime toDate)
        {
            _issuesService.FromDate = fromDate;
            _issuesService.ToDate = toDate;

            var issues = _issuesService.P1Issue;
            if (issues != null)
            {
                issues = issues.Where(t => !ProjectIdIgnore.Contains(t.Project.Id))
                    .OrderByDescending(t => t.IncidentDate).ToList();
                return GenerateProblemModel(issues, isDaily: false);
            }

            return new List<ProblemReportModel>();
        }

        public IList<ProblemReportModel> GetDailyProblemPendingReport()
        {
            var issues = _issuesService.PendingIssueDaily;
            if (issues != null)
            {
                issues = issues.Where(t => !ProjectIdIgnore.Contains(t.Project.Id))
                    .Where(t => !((t.CreatedDate - DateTime.Now.Date.AddDays(-1)).Days == 0 && t.Priority.Id == 1))
                    .ToList();
                return GenerateProblemModel(issues);
            }

            return new List<ProblemReportModel>();
        }

        public IList<ProblemReportModel> GetWeeklyProblemPendingReport(DateTime toDate)
        {
            var issues = _issuesService.PendingIssue;
            if (issues != null)
            {
                issues = issues.Where(t => !ProjectIdIgnore.Contains(t.Project.Id))
                    .OrderByDescending(t => t.IncidentDate).ToList();
                var problemModel = GenerateProblemModel(issues, isDaily: false);
                return problemModel;
            }

            return new List<ProblemReportModel>();
        }

        private static string GetPriorityName(int priorityId)
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