namespace Fanex.IncidentManagementReport.Services.Model
{
    using Core.Extensions;
    using System;
    using System.Collections.Generic;

    public class IncidentReportViewModel
    {
        public IList<IncidentReportModel> CreateNewIncidentReport { get; set; }

        public IList<IncidentReportModel> ClosedIncidentReport { get; set; }

        public IList<IncidentReportModel> PendingIncidentReport { get; set; }

        public IList<IncidentReportModel> P1IncidentReport { get; set; }

        public IList<IncidentReportModel> AgingDataLastWeek { get; set; }

        public IList<IncidentReportModel> AgingDataLast2Week { get; set; }

        public string RedmineUrl { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string FromDateDisplay
        {
            get { return Utilities.GetDisplayShortDate(FromDate); }
        }

        public string ToDateDisplay
        {
            get { return Utilities.GetDisplayShortDate(ToDate); }
        }

        public string FromLast2WeekDisplay
        {
            get { return Utilities.GetDisplayShortDate(FromDate.AddDays(-7)); }
        }

        public string ToLast2WeekDisplay
        {
            get { return Utilities.GetDisplayShortDate(ToDate.AddDays(-7)); }
        }

        public IncidentPrioritySummary CreateNewIncidentSummary { get; set; }

        public IncidentPrioritySummary ClosedIncidentSummary { get; set; }

        public IncidentPrioritySummary PendingIncidentSummary { get; set; }

        public IncidentPrioritySummary AgingDataLastWeekSummary
        {
            get { return new IncidentPrioritySummary(AgingDataLastWeek); }
        }

        public IncidentPrioritySummary AgingDataLast2WeekSummary
        {
            get { return new IncidentPrioritySummary(AgingDataLast2Week); }
        }
    }
}