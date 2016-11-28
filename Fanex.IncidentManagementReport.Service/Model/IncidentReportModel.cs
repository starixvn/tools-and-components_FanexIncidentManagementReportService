namespace Fanex.IncidentManagementReport.Service.Model
{
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
    }
}
