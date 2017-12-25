namespace Fanex.IncidentManagementReport.Service.Model
{
    using System;
    using System.Collections.Generic;
    using Core;

    public class ProblemReportViewModel
    {
        public IList<ProblemReportModel> NewCreatedProblems { get; set; }

        public IList<ProblemReportModel> ClosedProblems { get; set; }

        /// <summary>
        /// Including Assignee, Workaround, Pending Changes status
        /// </summary>
        public IList<ProblemReportModel> PendingProblems { get; set; }

        public IList<ProblemReportModel> P1Problems { get; set; }

        public string RedmineUrl { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string FromDateDisplay => Utilities.GetDisplayShortDate(FromDate);

        public string ToDateDisplay => Utilities.GetDisplayShortDate(ToDate);

        public string FromLast2WeekDisplay => Utilities.GetDisplayShortDate(FromDate.AddDays(-7));

        public string ToLast2WeekDisplay => Utilities.GetDisplayShortDate(ToDate.AddDays(-7));

        public ProblemPrioritySummary NewCreatedProblemSummary { get; set; }

        public ProblemPrioritySummary OpenProblemsSummary { get; set; }

        public ProblemPrioritySummary WorkaroundProblemSummary { get; set; }

        public ProblemPrioritySummary PendingChangeProblemSummary { get; set; }

        public ProblemPrioritySummary ClosedProblemSummary { get; set; }
}
}