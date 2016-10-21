using System.Collections.Generic;
using Fanex.IncidentManagementReport.Service.Model;

namespace Fanex.IncidentManagementReport.Service.Services
{
    public interface IIssuesService
    {
        IList<IssuesViewModel> CreateNewIssue { get; }

        IList<IssuesViewModel> ChangedIssue { get; }

        IList<IssuesViewModel> PendingIssue { get; }
    }
}
