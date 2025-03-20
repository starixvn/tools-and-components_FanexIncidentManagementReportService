namespace Fanex.IncidentManagementReport.Services.Models;

using System.Collections.Generic;
using System.Linq;
using Core.Enums;

public class IncidentPrioritySummary
{
    public IncidentPrioritySummary(IList<IncidentReportModel> incidents)
    {
        GetIncidentPrioritySummary(incidents);
    }

    public int P1Count { get; set; }

    public int P2Count { get; set; }

    public int P3Count { get; set; }

    private void GetIncidentPrioritySummary(IList<IncidentReportModel> incidents)
    {
        if (incidents != null && incidents.Any())
        {
            P1Count = incidents.Where(t => t.PriorityId == (int)Priorities.P1).Count();
            P2Count = incidents.Where(t => t.PriorityId == (int)Priorities.P2).Count();
            P3Count = incidents.Where(t => t.PriorityId == (int)Priorities.P3).Count();
        }
    }
}