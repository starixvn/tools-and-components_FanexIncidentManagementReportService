using System.Collections.Generic;
using Fanex.IncidentManagementReport.Service.Model;

namespace Fanex.IncidentManagementReport.Service.Services
{
    public interface IIncidentReportService
    {
        IList<IncidentReportModel> GetIncidentWeeklyCreateNewReport();

        IList<IncidentReportModel> GetIncidentWeeklyClosedReport();

        IList<IncidentReportModel> GetIncidentWeeklyPendingReport();

        IList<IncidentReportModel> GetIncidentWeeklyP1Report();

        string GetRedmineUrl();
    }
}
