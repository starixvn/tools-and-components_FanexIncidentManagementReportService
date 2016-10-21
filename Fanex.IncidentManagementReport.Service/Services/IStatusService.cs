using System.Collections.Generic;
using Fanex.IncidentManagementReport.Service.Model;

namespace Fanex.IncidentManagementReport.Service.Services
{
    public interface IStatusService
    {
        IList<StatusViewModel> Status { get; }
    }
}
