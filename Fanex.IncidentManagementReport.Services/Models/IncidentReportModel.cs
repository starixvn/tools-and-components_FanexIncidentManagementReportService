namespace Fanex.IncidentManagementReport.Services.Models;

using Core.Extensions;
using System;

public class IncidentReportModel
{
    public int IncidentId { get; set; }

    public string CategoryName { get; set; }

    public string IncidentTitle { get; set; }

    public int PriorityId { get; set; }

    public string Priority { get; set; }

    public string Creator { get; set; }

    public string Assignee { get; set; }

    public string Status { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime IncidentDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ResolvedDate { get; set; }

    public DateTime ClosedDate { get; set; }

    public string IncidentDateDisplay => IncidentDate.Year == 1 ? "N/A" : Utilities.GetDisplayLongDate(IncidentDate);

    public string CreatedDateDisplay => Utilities.GetDisplayLongDate(CreatedDate);

    public string ResolvedDateDisplay
    {
        get
        {
            var dateDisplay = ResolvedDate.Year == 1 ? "N/A" : Utilities.GetDisplayLongDate(ResolvedDate);
            return dateDisplay;
        }
    }

    public string ClosedDateDisplay
    {
        get
        {
            var dateDisplay = ClosedDate.Year == 1 ? "N/A" : Utilities.GetDisplayLongDate(ClosedDate);
            return dateDisplay;
        }
    }
}