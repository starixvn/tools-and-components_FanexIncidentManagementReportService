namespace Fanex.IncidentManagementReport.Services.Models;

using Core.Enums;
using System.Collections.Generic;
using System.Linq;

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
            P1Count = incidents.Count(t => t.PriorityId == (int)Priorities.P1);
            P2Count = incidents.Count(t => t.PriorityId == (int)Priorities.P2);
            P3Count = incidents.Count(t => t.PriorityId == (int)Priorities.P3);
        }
    }
}