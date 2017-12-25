

namespace Fanex.IncidentManagementReport.Service.Services
{
    using System;
    using System.Collections.Generic;
    using Model;
    using Model.Enumerations;

    public interface IIssuesService
    {
        IList<IssuesViewModel> CreateNewIssue { get; }

        IList<IssuesViewModel> CloseIssue { get; }

        IList<IssuesViewModel> PendingIssue { get; }

        IList<IssuesViewModel> PendingIssueDaily { get; }

        IList<IssuesViewModel> P1Issue { get; }

        IList<IssuesViewModel> P1IssueDaily { get; }

        DateTime FromDate { get; set; }

        DateTime ToDate { get; set; }

        Tracker Tracker { get; set; }
    }
}
