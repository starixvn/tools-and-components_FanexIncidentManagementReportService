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

        IList<IssuesViewModel> P1Issue { get; }

        string FromDate { get; }

        string ToDate { get; }

        string NowDate { get; }
    }
}
