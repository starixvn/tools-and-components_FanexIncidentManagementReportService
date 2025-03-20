namespace Fanex.IncidentManagementReport.Services.Core.Services.Interfaces
{
    using Enums;
    using Model;
    using Model.Problem;

    public interface IIssueService
    {
        Task<IList<IssuesViewModel>> GetCreateNewIssuesAsync(DateTime fromDate, DateTime toDate, TrackerEnum tracker);

        Task<IList<IssuesViewModel>> GetClosedIssuesAsync(DateTime fromDate, DateTime toDate, TrackerEnum tracker);

        Task<IList<IssuesViewModel>> GetPendingIssuesAsync(DateTime startDate, TrackerEnum tracker);

        Task<IList<IssuesViewModel>> GetPendingIssuesDailyAsync(TrackerEnum tracker);

        Task<IList<IssuesViewModel>> GetP1IssuesAsync(DateTime fromDate, DateTime toDate, TrackerEnum tracker);

        Task<IList<IssuesViewModel>> GetP1IssuesDailyAsync(DateTime fromDate, DateTime toDate, TrackerEnum tracker);

        Task<IList<ProblemIssue>> GetAllIssuesAsync(TrackerEnum tracker);

        Task<ProblemIssue> GetIssueByIdAsync(string issueId);
    }
}