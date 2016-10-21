using System.Collections.Generic;
using Fanex.IncidentManagementReport.Service.Model;

namespace Fanex.IncidentManagementReport.Service.Services
{
    public interface IIncidentReportService
    {
        IList<IncidentReportModel> GetIncidentDailyCreateNewReport();

        IList<IncidentReportModel> GetIncidentDailyUpdateReport();

        IList<IncidentReportModel> GetIncidentDailyPendingReport();

        string GetRedmineUrl();
    }
}
