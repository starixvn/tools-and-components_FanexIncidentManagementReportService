using System;
using System.Collections.Generic;
using Fanex.IncidentManagementReport.Service.Model;

namespace Fanex.IncidentManagementReport.Service.Services
{
    public interface IIssuesService
    {
        IList<IssuesViewModel> CreateNewIssue { get; }

        IList<IssuesViewModel> CloseIssue { get; }

        IList<IssuesViewModel> PendingIssue { get; }

        IList<IssuesViewModel> PendingIssueDaily { get; }

        IList<IssuesViewModel> P1Issue { get; }

        DateTime FromDate { get; set; }

        DateTime ToDate { get; set; }
    }
}
