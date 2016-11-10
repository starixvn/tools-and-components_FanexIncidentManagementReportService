using System;
using System.Collections.Generic;
using Fanex.IncidentManagementReport.Service.Model;

namespace Fanex.IncidentManagementReport.Service.Services
{
    public interface IIncidentReportService
    {
        IList<IncidentReportModel> GetIncidentCreateNewReport(DateTime fromDate, DateTime toDate);

        IList<IncidentReportModel> GetIncidentClosedReport(DateTime fromDate, DateTime toDate);

        IList<IncidentReportModel> GetIncidentPendingReport();

        IList<IncidentReportModel> GetWeeklyIncidentPendingReport(DateTime toDate);

        IList<IncidentReportModel> GetIncidentP1Report(DateTime fromDate, DateTime toDate);

        string GetRedmineUrl();
    }
}
