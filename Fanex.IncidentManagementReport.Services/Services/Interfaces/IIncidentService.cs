namespace Fanex.IncidentManagementReport.Services.Services.Interfaces;

using Models;

public interface IIncidentService
{
    Task<IList<IncidentReportModel>> GetCreateNewIncidentAsync(DateTime fromDate, DateTime toDate);

    Task<IList<IncidentReportModel>> GetClosedIncidentAsync(DateTime fromDate, DateTime toDate);

    Task<IList<IncidentReportModel>> GetP1IncidentDailyAsync(DateTime fromDate, DateTime toDate);

    Task<IList<IncidentReportModel>> GetP1IncidentWeeklyAsync(DateTime fromDate, DateTime toDate);

    Task<IList<IncidentReportModel>> GetPendingIncidentDailyAsync();

    Task<IList<IncidentReportModel>> GetPendingIncidentWeeklyAsync(DateTime toDate);
}