namespace Fanex.IncidentManagementReport.Service.Model
{
    using Core;
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
                string dateDisplay = ResolvedDate.Year == 1 ? "N/A" : Utilities.GetDisplayLongDate(ResolvedDate);
                return dateDisplay;
            }
        }

        public string ClosedDateDisplay
        {
            get
            {
                string dateDisplay = ClosedDate.Year == 1 ? "N/A" : Utilities.GetDisplayLongDate(ClosedDate);
                return dateDisplay;
            }
        }
    }
}