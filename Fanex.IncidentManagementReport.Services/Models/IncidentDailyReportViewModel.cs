namespace Fanex.IncidentManagementReport.Services.Models;

using System;
using System.Collections.Generic;
using System.Linq;

public class IncidentDailyReportViewModel
{
    public IList<IncidentGroup> PendingIncidents { get; set; }

    public IList<IncidentGroup> P1Incidents { get; set; }

    public string RedmineUrl { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public int PendingIncidentCount
    {
        get
        {
            var count = 0;
            foreach (var pendingIncident in PendingIncidents)
            {
                count += pendingIncident.Incidents.Count();
            }

            return count;
        }
    }

    public int P1IncidentCount
    {
        get
        {
            var count = 0;
            foreach (var priority1Incident in P1Incidents)
            {
                count += priority1Incident.Incidents.Count();
            }

            return count;
        }
    }
}