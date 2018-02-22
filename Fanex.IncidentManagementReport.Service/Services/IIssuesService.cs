namespace Fanex.IncidentManagementReport.Service.Services
{
    using System;
    using System.Collections.Generic;
    using Model;
    using Model.Problem;

    public interface IIssuesService
    {
        IList<IssuesViewModel> CreateNewIssue { get; }

        IList<IssuesViewModel> CloseIssue { get; }

        IList<IssuesViewModel> PendingIssue { get; }

        IList<IssuesViewModel> PendingIssueDaily { get; }

        IList<IssuesViewModel> P1Issue { get; }

        IList<IssuesViewModel> P1IssueDaily { get; }

        IList<Issue> GetAllIssues(TrackerEnum tracker);

        Issue GetSingleIssue(string issueId);

        DateTime FromDate { get; set; }

        DateTime ToDate { get; set; }

        TrackerEnum Tracker { get; set; }
    }
}
