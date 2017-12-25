namespace Fanex.IncidentManagementReport.Service.Services
{
    using System;
    using System.Collections.Generic;
    using Model;

    public interface IProblemReportService
    {
        IList<ProblemReportModel> GetProblemCreateNewReport(DateTime fromDate, DateTime toDate);

        IList<ProblemReportModel> GetProblemClosedReport(DateTime fromDate, DateTime toDate);

        IList<ProblemReportModel> GetDailyProblemP1Report(DateTime fromDate, DateTime toDate);

        IList<ProblemReportModel> GetWeeklyProblemP1Report(DateTime fromDate, DateTime toDate);

        IList<ProblemReportModel> GetDailyProblemPendingReport();

        IList<ProblemReportModel> GetWeeklyProblemPendingReport(DateTime toDate);

        string GetRedmineUrl();
    }
}