namespace Fanex.IncidentManagementReport.Services.Services;

using Core.Enums;
using Core.Extensions;
using Fanex.IncidentManagementReport.Services.Core.Models;
using Fanex.IncidentManagementReport.Services.Core.Services;
using Fanex.IncidentManagementReport.Services.Core.Services.Interfaces;
using Models.Problem;

public interface IProblemManagementReportService
{
    Task<string> GenerateProblemWeeklyReportContent();
}

public class ProblemManagementReportService(
    IIssueService issueService,
    IViewRenderService viewRenderService,
    RedmineApiSettings redmineSettings)
    : IProblemManagementReportService
{
    private const string ProblemWeeklyReportTemplate = "ProblemWeeklyReportTemplate";

    public async Task<string> GenerateProblemWeeklyReportContent()
    {
        var viewModel = await GetDataAsync();

        var content = await viewRenderService.RenderViewToStringAsync(ProblemWeeklyReportTemplate, viewModel);
        return content;
    }

    private async Task<WeeklyProblemsReportViewModel> GetDataAsync()
    {
        var issues = await issueService.GetAllIssuesAsync(TrackerEnum.Problem);
        // Get occurred time of related issue
        foreach (var issue in issues)
        {
            if (issue.Relations.Any())
            {
                foreach (var relation in issue.Relations)
                {
                    relation.DisplayedId = issue.Id != relation.IssueId ? relation.IssueId : relation.IssueToId;

                    var relatedIssue = await issueService.GetIssueByIdAsync(relation.DisplayedId.ToString());

                    relation.Tracker = (TrackerEnum)relatedIssue.Tracker.Id;
                    if (relatedIssue.ReportedDateTime != null && relation.Tracker is TrackerEnum.Incident)
                    {
                        relation.ReportedDateTime = relatedIssue.ReportedDateTime.Value;
                    }
                }
            }
        }

        var viewModel = new WeeklyProblemsReportViewModel
        {
            FromDate = DateTime.Now.DateOfPreviousWeek(DayOfWeek.Monday),
            ToDate = DateTime.Now.DateOfPreviousWeek(DayOfWeek.Sunday),
            RedmineUrl = redmineSettings.BaseUrl,
            IssueGroupsByStatus = new List<IssueGroup>(),
            IssueGroupsByTimeSummary = new List<IssueGroup>()
        };

        SplitProblemIntoGroupsByStatus(issues, viewModel);

        SplitProblemIntoGroupsByTimeSummary(issues, viewModel);

        return viewModel;
    }

    private static void SplitProblemIntoGroupsByStatus(IEnumerable<ProblemIssue> issues, WeeklyProblemsReportViewModel viewModel)
    {
        // Open
        viewModel.IssueGroupsByStatus.Add(
            new IssueGroup(
                1,
                issues,
                [(int)TrackerEnum.Problem],
                [
                    (int)StatusEnum.New,
                    (int)StatusEnum.Assigned,
                    (int)StatusEnum.Workaround,
                    (int)StatusEnum.PendingChange,
                    (int)StatusEnum.Resolved
                ],
                "Open Problems ({0}) - Include New, Assigned, Workaround, Pending Change, Resolved status until now"));

        // Resolved
        var filteredIssue = issues.Where(x => viewModel.FromDate <= x.ResolvedDate && x.ResolvedDate <= viewModel.ToDate);
        viewModel.IssueGroupsByStatus.Add(
            new IssueGroup(
                2,
                filteredIssue,
                [(int)TrackerEnum.Problem],
                [(int)StatusEnum.Resolved],
                "Resolved Problems ({0}) - Problems resolved last week"));

        // Closed
        filteredIssue = issues.Where(x => viewModel.FromDate <= x.CloseOn && x.CloseOn <= viewModel.ToDate);
        viewModel.IssueGroupsByStatus.Add(
            new IssueGroup(
                3,
                filteredIssue,
                [(int)TrackerEnum.Problem],
                [(int)StatusEnum.Closed],
                "Closed Problems ({0}) - Problems closed last week"));

        // Cancelled
        filteredIssue = issues.Where(x => viewModel.FromDate <= x.CloseOn && x.CloseOn <= viewModel.ToDate);
        viewModel.IssueGroupsByStatus.Add(
            new IssueGroup(
                4,
                filteredIssue,
                [(int)TrackerEnum.Problem],
                [(int)StatusEnum.Cancelled],
                "Cancelled Problems ({0}) - Problems cancelled last week"));

        // Newly Created
        filteredIssue = issues.Where(x => viewModel.FromDate <= x.CreatedOn && x.CreatedOn <= viewModel.ToDate);
        viewModel.IssueGroupsByStatus.Add(
            new IssueGroup(
                5,
                filteredIssue,
                "Newly created Problems ({0}) - Problems created last week"));
    }

    private static void SplitProblemIntoGroupsByTimeSummary(IEnumerable<ProblemIssue> issues, WeeklyProblemsReportViewModel viewModel)
    {
        // Open all
        viewModel.IssueGroupsByTimeSummary.Add(new IssueGroup(
            1,
            issues,
            [(int)TrackerEnum.Problem],
            [
                (int)StatusEnum.New,
                (int)StatusEnum.Assigned,
                (int)StatusEnum.Workaround,
                (int)StatusEnum.PendingChange,
                (int)StatusEnum.Resolved
            ],
            "Open (All)"));

        // Last week
        var problems = issues.Where(x => x.Tracker.Id == (int)TrackerEnum.Problem);

        var lastWeekIssues = new List<ProblemIssue>();
        // Resolved Last week
        lastWeekIssues.AddRange(problems.Where(x => x.Status.Id == (int)StatusEnum.Resolved
                                                    && x.ResolvedDate != null
                                                    && viewModel.FromDate <= x.ResolvedDate
                                                    && x.ResolvedDate <= viewModel.ToDate));

        // Closed / Cancelled Last week
        lastWeekIssues.AddRange(problems.Where(x => (x.Status.Id == (int)StatusEnum.Closed || x.Status.Id == (int)StatusEnum.Cancelled)
                                                    && x.CloseOn != null
                                                    && viewModel.FromDate <= x.CloseOn
                                                    && x.CloseOn <= viewModel.ToDate));

        // Newly created Last week
        lastWeekIssues.AddRange(problems.Where(x => viewModel.FromDate <= x.CreatedOn
                                                    && x.CreatedOn <= viewModel.ToDate));

        viewModel.IssueGroupsByTimeSummary.Add(
            new IssueGroup(2, lastWeekIssues)
            {
                Name = $"Last week ({viewModel.FromDate} - {viewModel.ToDate})"
            });

        // Aging data
        var filteredIssue = problems.Where(x => x.Status.Id != (int)StatusEnum.Closed
                                                && x.Status.Id != (int)StatusEnum.Cancelled
                                                && ((DateTime.Now - x.CreatedOn) >= TimeSpan.FromDays(60)));

        viewModel.IssueGroupsByTimeSummary.Add(new IssueGroup(3, filteredIssue, "Aging ({0})"));
    }
}