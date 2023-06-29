namespace Fanex.IncidentManagementReport.Services.Model.Problem
{
    using System;
    using System.Collections.Generic;

    public class WeeklyProblemsReportViewModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string RedmineUrl { get; set; }

        public IList<IssueGroup> IssueGroupsByStatus { get; set; }

        public IList<IssueGroup> IssueGroupsByTimeSummary { get; set; }
    }
}