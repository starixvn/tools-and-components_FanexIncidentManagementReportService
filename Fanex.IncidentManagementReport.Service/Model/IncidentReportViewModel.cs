using System;
using System.Collections.Generic;

namespace Fanex.IncidentManagementReport.Service.Model
{
    public class IncidentReportViewModel
    {
        public IList<IncidentReportModel> CreateNewIncidentReport { get; set; }

        public IList<IncidentReportModel> ClosedIncidentReport { get; set; }
      
        public IList<IncidentReportModel> PendingIncidentReport { get; set; }

        public IList<IncidentReportModel> P1IncidentReport { get; set; }

        public string RedmineUrl { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}