namespace Fanex.IncidentManagementReport.Service
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core;
    using Model;
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
            var fromDate = DateTime.Now.DateOfPreviousWeek(DayOfWeek.Monday);
            var toDate = DateTime.Now.DateOfPreviousWeek(DayOfWeek.Sunday);

            return ProblemReportGeneration(fromDate, toDate, "EmailTemplates/ProblemWeeklyReportTemplate.html");
        }

        private IList<ProblemGroup> GetP1ProblemCategory(DateTime fromDate, DateTime toDate)
        {
            var priority1Problems = problemReportService.GetDailyProblemP1Report(fromDate, toDate);
            IList<ProblemReportModel> priority1GroupByCategory = priority1Problems.GroupBy(t => t.CategoryName).Select(grp => grp.First()).ToList();
            IList<ProblemGroup> priority1ProblemGroups = priority1GroupByCategory.Select(t => new ProblemGroup()
            {
                CategoryName = t.CategoryName,
                Problems = priority1Problems.Where(x => x.CategoryName == t.CategoryName).ToList()
            }).ToList();

            return priority1ProblemGroups;
        }

        private IList<ProblemGroup> GetPendingProblemCategory()
        {
            var pendingProblems = problemReportService.GetDailyProblemPendingReport();
            IList<ProblemReportModel> pendingProblemGroupByCategory = pendingProblems.GroupBy(t => t.CategoryName).Select(grp => grp.First()).ToList();
            IList<ProblemGroup> pendingProblemGroups = pendingProblemGroupByCategory.Select(t => new ProblemGroup()
            {
                CategoryName = t.CategoryName,
                Problems = pendingProblems.Where(x => x.CategoryName == t.CategoryName).ToList()
            }).ToList();

            return pendingProblemGroups;
        }

        private string ProblemReportGeneration(DateTime fromDate, DateTime toDate, string emailTemplatePath)
        {
            var newProblems = problemReportService.GetProblemCreateNewReport(fromDate, toDate);
            var closedProblems = problemReportService.GetProblemClosedReport(fromDate, toDate);
            var pendingProblems = problemReportService.GetWeeklyProblemPendingReport(toDate);
            var firstPriorityProblems = problemReportService.GetWeeklyProblemP1Report(fromDate, toDate);

            var newProblemSummary = new ProblemPrioritySummary(newProblems);
            var closedProblemSummary = new ProblemPrioritySummary(closedProblems);
            var pendingProblemSummary = new ProblemPrioritySummary(pendingProblems);

            var getRedmineUrl = problemReportService.GetRedmineUrl();
            var model = new ProblemReportViewModel
            {
                NewCreatedProblemSummary = newProblemSummary,
                ClosedProblemSummary = closedProblemSummary,
                PendingChangeProblemSummary = pendingProblemSummary,
                NewCreatedProblems = RemoveP1Problems(firstPriorityProblems, newProblems),
                ClosedProblems = RemoveP1Problems(firstPriorityProblems, closedProblems),
                PendingProblems = RemoveP1Problems(firstPriorityProblems, pendingProblems),
                P1Problems = firstPriorityProblems,
                RedmineUrl = getRedmineUrl + "/issues/",
                FromDate = fromDate,
                ToDate = toDate
            };

            var templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, emailTemplatePath);
            var emailTemplate = File.ReadAllText(templateFolderPath);

            var content = Engine.Razor.RunCompile(emailTemplate, Path.GetFileName(emailTemplatePath) + "v6", typeof(ProblemReportViewModel), model);

            return content;
        }

        private static IList<ProblemReportModel> RemoveP1Problems(IList<ProblemReportModel> firstPriorityProblems, IList<ProblemReportModel> problems)
        {
            problems = problems.Where(t => firstPriorityProblems.All(x => x.ProblemId != t.ProblemId)).ToList();
            return problems;
        }

        private IList<ProblemReportModel> CombineProblems(
            IList<ProblemReportModel> firstPriorityProblems, 
            IList<ProblemReportModel> newProblems, 
            IList<ProblemReportModel> closedProblems, 
            IList<ProblemReportModel> pendingProblems)
        {
            List<ProblemReportModel> problems = new List<ProblemReportModel>();

            problems.AddRange(firstPriorityProblems);
            problems.AddRange(newProblems.Where(t => problems.All(x => x.ProblemId != t.ProblemId)));
            problems.AddRange(closedProblems.Where(t => problems.All(x => x.ProblemId != t.ProblemId)));
            problems.AddRange(pendingProblems.Where(t => problems.All(x => x.ProblemId != t.ProblemId)));

            return problems;
        }

        private IList<ProblemReportModel> GetAgingProblems(IList<ProblemReportModel> problems, DateTime fromDate, DateTime toDate)
        {
            problems = problems.Where(t => !IsCancelled(t) && !IsClosedInPeriod(t, fromDate, toDate)).ToList();

            return problems;
        }

        private static bool IsCancelled(ProblemReportModel problem)
        {
            return string.Equals(problem.Status, "Cancelled");
        }

        private static bool IsClosedInPeriod(ProblemReportModel problem, DateTime fromDate, DateTime toDate)
        {
            var isClosedInPeriod = false;

            if (string.Equals(problem.Status, "Closed") && problem.ResolvedDate.Year != 1)
            {
                isClosedInPeriod = problem.ResolvedDate >= fromDate && problem.ResolvedDate <= toDate;
            }

            return isClosedInPeriod;
        }
    }
}