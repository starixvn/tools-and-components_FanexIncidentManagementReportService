using System;

namespace Fanex.IncidentManagementReport.Service.Model
{
    public class IncidentReportModel
    {
        public int IncidentId { get; set; }

        public string CategoryName { get; set; }

        public string IncidentTitle { get; set; }

        public string Priority { get; set; }

        public string Creator { get; set; }

        public string Assignee { get; set; }

        public string Status { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime IncidentDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ResolvedDate { get; set; }
    }
}
