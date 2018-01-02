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
                        (int)StatusEnum.PendingChange
                    },
                    "Open Problems ({0}) - Include New, Assinged, Workaround, Pending Change status untill now"));

            // Resolved
            var filteredIssue = issues.Where(x => viewModel.FromDate <= x.ResolvedDate && x.ResolvedDate <= viewModel.ToDate);
            viewModel.IssueGroupsByStatus.Add(
                new IssueGroup(
                    2,
                    filteredIssue,
                    new[] { (int)TrackerEnum.Problem },
                    new[] { (int)StatusEnum.Resolved },
                    "Resolved Problems ({0})"));

            // Closed
            filteredIssue = issues.Where(x => viewModel.FromDate <= x.CloseOn && x.CloseOn <= viewModel.ToDate);
            viewModel.IssueGroupsByStatus.Add(
                new IssueGroup(
                    3,
                    filteredIssue,
                    new[] { (int)TrackerEnum.Problem },
                    new[] { (int)StatusEnum.Closed },
                    "Closed Problems ({0})"));

            // Cancelled
            filteredIssue = issues.Where(x => viewModel.FromDate <= x.CloseOn && x.CloseOn <= viewModel.ToDate);
            viewModel.IssueGroupsByStatus.Add(
                new IssueGroup(
                    4,
                    filteredIssue,
                    new[] { (int)TrackerEnum.Problem },
                    new[] { (int)StatusEnum.Cancelled },
                    "Cancelled Problems ({0})"));

            // Newly Created
            filteredIssue = issues.Where(x => viewModel.FromDate <= x.CreatedOn && x.CreatedOn <= viewModel.ToDate);
            viewModel.IssueGroupsByStatus.Add(
                new IssueGroup(
                    5,
                    filteredIssue,
                    "Newly created Problem Report ({0})"));
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
                    (int)StatusEnum.PendingChange
                },
                "Open (All)"));

            // Lastweek
            viewModel.IssueGroupsByTimeSummary.Add(new IssueGroup(2, issues, viewModel.FromDate, viewModel.ToDate, "Last week ({0} - {1}"));

            // Aging data
            var filteredIssue = issues.Where(x => x.Tracker.Id == (int)TrackerEnum.Problem);
            filteredIssue = filteredIssue.Where(x =>
                x.Status.Id != (int)StatusEnum.Closed && x.Status.Id != (int)StatusEnum.Cancelled);
            // (Resolved Date - Created Date) >= 2 Months
            ////filteredIssue = filteredIssue.Where(x =>
            ////    (x.ResolvedDate != null && (x.ResolvedDate.Value - x.CreatedOn) >= TimeSpan.FromDays(60))
            ////    || (x.ResolvedDate == null && (DateTime.Now - x.CreatedOn) >= TimeSpan.FromDays(60)));

            filteredIssue = filteredIssue.Where(x => ((DateTime.Now - x.CreatedOn) >= TimeSpan.FromDays(60)));

            viewModel.IssueGroupsByTimeSummary.Add(new IssueGroup(
                3,
                filteredIssue,
                "Aging ({0})"));
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