namespace Fanex.IncidentManagementReport.Service.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.IncidentManagementReport.Service.Model;

    public interface IIncidentReportService
    {
        IList<IncidentReportModel> GetIncidentCreateNewReport(DateTime fromDate, DateTime toDate);

        IList<IncidentReportModel> GetIncidentClosedReport(DateTime fromDate, DateTime toDate);

        IList<IncidentReportModel> GetDailyIncidentP1Report(DateTime fromDate, DateTime toDate);

        IList<IncidentReportModel> GetWeeklyIncidentP1Report(DateTime fromDate, DateTime toDate);

        IList<IncidentReportModel> GetDailyIncidentPendingReport();

        IList<IncidentReportModel> GetWeeklyIncidentPendingReport(DateTime toDate);

        string GetRedmineUrl();
    }
}
