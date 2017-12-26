namespace Fanex.IncidentManagementReport.Service
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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

        private WeeklyProblemsReportViewModel GetData()
        {
            var fromDate = DateTime.Now.DateOfPreviousWeek(DayOfWeek.Monday);
            var toDate = DateTime.Now.DateOfPreviousWeek(DayOfWeek.Sunday);

            // TODO: For test
            fromDate = new DateTime(2017, 12, 10);

            // Get all problems
            var issues = problemReportService.GetAllProblems();

            var viewModel = new WeeklyProblemsReportViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                RedmineUrl = problemReportService.GetRedmineUrl(),
                IssueGroupsByStatus = new List<IssueGroup>()
            };

            SplitProblemIntoGroups(issues, viewModel);

            viewModel.IssueGroupsByTimeSummary = new List<IssueGroup>();

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
            viewModel.IssueGroupsByTimeSummary.Add(new IssueGroup(
                2,
                issues,
                DateTime.Now.DateOfPreviousWeek(DayOfWeek.Monday),
                DateTime.Now.DateOfPreviousWeek(DayOfWeek.Sunday),
                "Last week ({0} - {1}"));

            // Aging data
            // TODO: Filter not correct here
            viewModel.IssueGroupsByTimeSummary.Add(new IssueGroup(
                2,
                issues,
                DateTime.Now.DateOfPreviousWeek(DayOfWeek.Monday),
                DateTime.Now.DateOfPreviousWeek(DayOfWeek.Sunday),
                "Last week ({0} - {1}"));

            return viewModel;
        }

        private static void SplitProblemIntoGroups(IEnumerable<Issue> issues, WeeklyProblemsReportViewModel viewModel)
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
            viewModel.IssueGroupsByStatus.Add(
                new IssueGroup(
                    2,
                    issues,
                    new[] { (int)TrackerEnum.Problem },
                    new[] { (int)StatusEnum.Resolved },
                    "Resolved Problems ({0})"));

            // Closed
            viewModel.IssueGroupsByStatus.Add(
                new IssueGroup(
                    3,
                    issues,
                    new[] { (int)TrackerEnum.Problem },
                    new[] { (int)StatusEnum.Closed },
                    "Closed Problems ({0})"));

            // Cancelled
            viewModel.IssueGroupsByStatus.Add(
                new IssueGroup(
                    4,
                    issues,
                    new[] { (int)TrackerEnum.Problem },
                    new[] { (int)StatusEnum.Cancelled },
                    "Cancelled Problems ({0})"));

            // Newly Created
            viewModel.IssueGroupsByStatus.Add(
                new IssueGroup(
                    5,
                    issues,
                    DateTime.Now.DateOfPreviousWeek(DayOfWeek.Monday),
                    DateTime.Now.DateOfPreviousWeek(DayOfWeek.Sunday),
                    "Newly created Problem Report ({0})"));
        }

        public string ProblemWeeklyReportEmailTemplate()
        {
            var viewModel = GetData();

            var emailTemplatePath = "EmailTemplates/ProblemWeeklyReportTemplate.html";

            var templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, emailTemplatePath);
            var emailTemplate = File.ReadAllText(templateFolderPath);

            return Engine.Razor.RunCompile(
                emailTemplate,
                Path.GetFileName(emailTemplatePath) + "v6",
                typeof(WeeklyProblemsReportViewModel), viewModel);
        }
    }
}