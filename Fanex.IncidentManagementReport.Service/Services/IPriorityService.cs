using System.Collections.Generic;
using Fanex.IncidentManagementReport.Service.Model;

namespace Fanex.IncidentManagementReport.Service.Services
{
    public interface IPriorityService
    {
        IList<PriorityViewModel> Priorities { get; }
    }
}
