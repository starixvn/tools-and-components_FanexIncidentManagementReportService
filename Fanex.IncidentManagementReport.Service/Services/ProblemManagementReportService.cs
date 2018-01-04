namespace Fanex.IncidentManagementReport.Service
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core;
    using Model.Problem;
    using RazorEngine;
    using RazorEngine.Templating;
    using Services;

    public class ProblemManagementReportService
    {
        private readonly IProblemReportService problemReportService;

        public ProblemManagementReportService() : this(null)
        {
        }

        public ProblemManagementReportService(IProblemReportService problemReportService)
        {
            this.problemReportService = problemReportService ?? new ProblemReportService();
        }

        public string ProblemWeeklyReportEmailTemplate()
        {
            var viewModel = new WeeklyProblemsReportViewModel();
            viewModel = GetData(viewModel.FromDate, viewModel.ToDate);

            var emailTemplatePath = "EmailTemplates/ProblemWeeklyReportTemplate.html";

            var templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, emailTemplatePath);
            var emailTemplate = File.ReadAllText(templateFolderPath);

            return Engine.Razor.RunCompile(
                emailTemplate,
                Path.GetFileName(emailTemplatePath) + "v6",
                typeof(WeeklyProblemsReportViewModel), viewModel);
        }

        private static void SplitProblemIntoGroupsByStatus(IEnumerable<Issue> issues, WeeklyProblemsReportViewModel viewModel)
        {
            // Open
            viewModel.IssueGroupsByStatus.Add(
                new IssueGroup(
                    1,
                    issues,
                    new[] { (int)TrackerEnum.Problem },
                    new[]
                    {
                        (int)StatusEnum.New,
                        (int)StatusEnum.Assigned,
                        (int)StatusEnum.Workaround,
                        (int)StatusEnum.PendingChange,
                        (int)StatusEnum.Resolved
                    },
                    "Open Problems ({0}) - Include New, Assigned, Workaround, Pending Change, Resolved status untill now"));

            // Resolved
            var filteredIssue = issues.Where(x => viewModel.FromDate <= x.ResolvedDate && x.ResolvedDate <= viewModel.ToDate);
            viewModel.IssueGroupsByStatus.Add(
                new IssueGroup(
                    2,
                    filteredIssue,
                    new[] { (int)TrackerEnum.Problem },
                    new[] { (int)StatusEnum.Resolved },
                    "Resolved Problems ({0}) - Problems resolved last week"));

            // Closed
            filteredIssue = issues.Where(x => viewModel.FromDate <= x.CloseOn && x.CloseOn <= viewModel.ToDate);
            viewModel.IssueGroupsByStatus.Add(
                new IssueGroup(
                    3,
                    filteredIssue,
                    new[] { (int)TrackerEnum.Problem },
                    new[] { (int)StatusEnum.Closed },
                    "Closed Problems ({0}) - Problems closed last week"));

            // Cancelled
            filteredIssue = issues.Where(x => viewModel.FromDate <= x.CloseOn && x.CloseOn <= viewModel.ToDate);
            viewModel.IssueGroupsByStatus.Add(
                new IssueGroup(
                    4,
                    filteredIssue,
                    new[] { (int)TrackerEnum.Problem },
                    new[] { (int)StatusEnum.Cancelled },
                    "Cancelled Problems ({0}) - Problems cancelled last week"));

            // Newly Created
            filteredIssue = issues.Where(x => viewModel.FromDate <= x.CreatedOn && x.CreatedOn <= viewModel.ToDate);
            viewModel.IssueGroupsByStatus.Add(
                new IssueGroup(
                    5,
                    filteredIssue,
                    "Newly created Problems ({0}) - Problems created last week"));
        }

        private static void SplitProblemIntoGroupsByTimeSummary(IEnumerable<Issue> issues, WeeklyProblemsReportViewModel viewModel)
        {
            // Open all
            viewModel.IssueGroupsByTimeSummary.Add(new IssueGroup(
                1,
                issues,
                new[] { (int)TrackerEnum.Problem },
                new[]
                {
                    (int)StatusEnum.New,
                    (int)StatusEnum.Assigned,
                    (int)StatusEnum.Workaround,
                    (int)StatusEnum.PendingChange,
                    (int)StatusEnum.Resolved
                },
                "Open (All)"));

            // Lastweek
            var problems = issues.Where(x => x.Tracker.Id == (int)TrackerEnum.Problem);

            var lastweekIssues = new List<Issue>();
            // Resolved Lastweek
            lastweekIssues.AddRange(problems.Where(x => x.Status.Id == (int)StatusEnum.Resolved
                                                        && x.ResolvedDate != null
                                                        && viewModel.FromDate <= x.ResolvedDate
                                                        && x.ResolvedDate <= viewModel.ToDate));

            // Closed / Cancelled Lastweek
            lastweekIssues.AddRange(problems.Where(x => (x.Status.Id == (int)StatusEnum.Closed || x.Status.Id == (int)StatusEnum.Cancelled)
                                                        && x.CloseOn != null
                                                        && viewModel.FromDate <= x.CloseOn
                                                        && x.CloseOn <= viewModel.ToDate));

            // Newly created Lastweek
            lastweekIssues.AddRange(problems.Where(x => viewModel.FromDate <= x.CreatedOn
                                                        && x.CreatedOn <= viewModel.ToDate));

            viewModel.IssueGroupsByTimeSummary.Add(
                new IssueGroup(2, lastweekIssues)
                {
                    Name = $"Last week ({viewModel.FromDate} - {viewModel.ToDate})"
                });

            // Aging data
            var filteredIssue = problems.Where(x => x.Status.Id != (int)StatusEnum.Closed
                                                    && x.Status.Id != (int)StatusEnum.Cancelled
                                                    && ((DateTime.Now - x.CreatedOn) >= TimeSpan.FromDays(60)));

            viewModel.IssueGroupsByTimeSummary.Add(new IssueGroup(3, filteredIssue, "Aging ({0})"));
        }

        private WeeklyProblemsReportViewModel GetData(DateTime fromDate, DateTime toDate)
        {
            if (fromDate == default(DateTime))
            {
                fromDate = DateTime.Now.DateOfPreviousWeek(DayOfWeek.Monday);
            }

            if (toDate == default(DateTime))
            {
                toDate = DateTime.Now.DateOfPreviousWeek(DayOfWeek.Sunday);
            }

            // Get all problems
            var issues = problemReportService.GetAllProblems();

            var viewModel = new WeeklyProblemsReportViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                RedmineUrl = problemReportService.GetRedmineUrl(),
                IssueGroupsByStatus = new List<IssueGroup>(),
                IssueGroupsByTimeSummary = new List<IssueGroup>()
            };

            SplitProblemIntoGroupsByStatus(issues, viewModel);

            SplitProblemIntoGroupsByTimeSummary(issues, viewModel);

            return viewModel;
        }
    }
}