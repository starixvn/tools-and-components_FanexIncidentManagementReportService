namespace Fanex.IncidentManagementReport.Service.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Core;

    public class ProblemReportModel
    {
        public int ProblemId { get; set; }

        public int PriorityId { get; set; }

        public string Priority { get; set; }

        public string CategoryName { get; set; }

        public string ProblemTitle { get; set; }
        
        public string Status { get; set; }

        [Display(Name = "Reported Date")]
        public DateTime ReportedDate { get; set; }

        [Display(Name = "Resolved Date")]
        public DateTime ResolvedDate { get; set; }

        [Display(Name = "Closed Date")]
        public DateTime ClosedDate { get; set; }

        public string Creator { get; set; }

        public string Assignee { get; set; }
        
        public string RelatedIncidents { get; set; }

        public string KnowError { get; set; }

        public string RequestForChange { get; set; }

        public string CreatedDateDisplay => Utilities.GetDisplayLongDate(ReportedDate);

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
}