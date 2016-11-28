namespace Fanex.IncidentManagementReport.Service.Model
{
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

        public IncidentPrioritySummary CreateNewIncidentSummary
        {
            get
            {
                return new IncidentPrioritySummary(CreateNewIncidentReport);
            }
        }

        public IncidentPrioritySummary ClosedIncidentSummary
        {
            get
            {
                return new IncidentPrioritySummary(ClosedIncidentReport);
            }
        }

        public IncidentPrioritySummary PendingIncidentSummary
        {
            get
            {
                return new IncidentPrioritySummary(PendingIncidentReport);
            }
        }

        public IncidentPrioritySummary AgingDataLastWeekSummary
        {
            get
            {
                return new IncidentPrioritySummary(AgingDataLastWeek);
            }
        }

        public IncidentPrioritySummary AgingDataLast2WeekSummary
        {
            get
            {
                return new IncidentPrioritySummary(AgingDataLast2Week);
            }
        }
    }
}