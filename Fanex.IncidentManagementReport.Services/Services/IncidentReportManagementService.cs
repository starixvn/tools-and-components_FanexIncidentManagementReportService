namespace Fanex.IncidentManagementReport.Services.Services
{
    using Core.Extensions;
    using Fanex.IncidentManagementReport.Service.Core;
    using Fanex.IncidentManagementReport.Services.Core.Models;
    using Fanex.IncidentManagementReport.Services.Core.Services;
    using FluentDateTime;
    using Interfaces;
    using Model;
    using System.Threading.Tasks;

    public interface IIncidentReportManagementService
    {
        Task<string> GenerateIncidentWeeklyReportContent();

        Task<string> GenerateIncidentDailyReportContent();
    }

    public class IncidentReportManagementService : IIncidentReportManagementService
    {
        private readonly IIncidentService incidentService;
        private readonly IViewRenderService viewRenderService;
        private readonly RedmineApiSettings redmineSettings;
        private const string WeeklyReportEmailTemplate = "IncidentWeeklyReportTemplate";
        private const string DailyReportEmailTemplate = "IncidentDailyReportTemplate";

        public IncidentReportManagementService(IIncidentService incidentService, IViewRenderService viewRenderService, RedmineApiSettings redmineSettings)
        {
            this.incidentService = incidentService;
            this.redmineSettings = redmineSettings;
            this.viewRenderService = viewRenderService;
        }

        public async Task<string> GenerateIncidentDailyReportContent()
        {
            var yesterday = DateTime.Now.PreviousDay();
            var content = await IncidentDailyReportGeneration(yesterday);
            return content;
        }

        public async Task<string> GenerateIncidentWeeklyReportContent()
        {
            var fromDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Monday);
            var toDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Sunday);
            var content = await IncidentWeeklyReportGeneration(fromDate, toDate);
            return content;
        }

        private async Task<string> IncidentWeeklyReportGeneration(DateTime fromDate, DateTime toDate)
        {
            var newIncidents = await incidentService.GetCreateNewIncidentAsync(fromDate, toDate);
            var closedIncidents = await incidentService.GetClosedIncidentAsync(fromDate, toDate);
            var pendingIncidents = await incidentService.GetPendingIncidentWeeklyAsync(toDate);
            var firstPriorityIncidents = await incidentService.GetP1IncidentWeeklyAsync(fromDate, toDate);

            var newIncidentSummary = new IncidentPrioritySummary(newIncidents);
            var closedIncidentSummary = new IncidentPrioritySummary(closedIncidents);
            var pendingIncidentSummary = new IncidentPrioritySummary(pendingIncidents);

            var agingIncidentsLastWeek = GetAgingIncidentsLastWeek(firstPriorityIncidents, newIncidents, closedIncidents, pendingIncidents, fromDate, toDate);
            var agingIncidentsLast2Week = await GetAgingIncidentsLast2Week(fromDate.AddDays(-7), toDate.AddDays(-7));

            var model = new IncidentReportViewModel()
            {
                CreateNewIncidentSummary = newIncidentSummary,
                ClosedIncidentSummary = closedIncidentSummary,
                PendingIncidentSummary = pendingIncidentSummary,
                CreateNewIncidentReport = RemoveP1Incidents(firstPriorityIncidents, newIncidents),
                ClosedIncidentReport = RemoveP1Incidents(firstPriorityIncidents, closedIncidents),
                PendingIncidentReport = RemoveP1Incidents(firstPriorityIncidents, pendingIncidents),
                P1IncidentReport = firstPriorityIncidents,
                AgingDataLastWeek = agingIncidentsLastWeek,
                AgingDataLast2Week = agingIncidentsLast2Week,
                RedmineUrl = redmineSettings.BaseUrl + "/issues/",
                FromDate = fromDate,
                ToDate = toDate
            };

            var content = await viewRenderService.RenderViewToStringAsync(WeeklyReportEmailTemplate, model);

            return content;
        }

        private IList<IncidentReportModel> GetAgingIncidentsLastWeek(
            IList<IncidentReportModel> firstPriorityIncidents,
            IList<IncidentReportModel> newIncidents,
            IList<IncidentReportModel> closedIncidents,
            IList<IncidentReportModel> pendingIncidents,
            DateTime fromDate,
            DateTime toDate)
        {
            var incidents = CombineIncidents(firstPriorityIncidents, newIncidents, closedIncidents, pendingIncidents);
            incidents = GetAgingIncidents(incidents, fromDate, toDate);
            return incidents;
        }

        private async Task<IList<IncidentReportModel>> GetAgingIncidentsLast2Week(DateTime fromDate, DateTime toDate)
        {
            var newIncidents = await incidentService.GetCreateNewIncidentAsync(fromDate, toDate);
            var closedIncidents = await incidentService.GetClosedIncidentAsync(fromDate, toDate);
            var pendingIncidents = await incidentService.GetPendingIncidentWeeklyAsync(toDate);
            var firstPriorityIncidents = await incidentService.GetP1IncidentWeeklyAsync(fromDate, toDate);

            var incidents = CombineIncidents(firstPriorityIncidents, newIncidents, closedIncidents, pendingIncidents);
            incidents = GetAgingIncidents(incidents, fromDate, toDate);
            return incidents;
        }

        private IList<IncidentReportModel> CombineIncidents(
            IList<IncidentReportModel> firstPriorityIncidents,
            IList<IncidentReportModel> newIncidents,
            IList<IncidentReportModel> closedIncidents,
            IList<IncidentReportModel> pendingIncidents)
        {
            List<IncidentReportModel> incidents = new();

            incidents.AddRange(firstPriorityIncidents);
            incidents.AddRange(newIncidents.Where(t => !incidents.Any(x => x.IncidentId == t.IncidentId)));
            incidents.AddRange(closedIncidents.Where(t => !incidents.Any(x => x.IncidentId == t.IncidentId)));
            incidents.AddRange(pendingIncidents.Where(t => !incidents.Any(x => x.IncidentId == t.IncidentId)));

            return incidents;
        }

        private IList<IncidentReportModel> GetAgingIncidents(IList<IncidentReportModel> incidents, DateTime fromDate, DateTime toDate)
        {
            incidents = incidents.Where(t => IsAgingIncident(t, toDate)).ToList();
            incidents = incidents.Where(t => !IsCancelled(t) && !IsClosedInPeriod(t, fromDate, toDate)).ToList();

            return incidents;
        }

        private IList<IncidentReportModel> RemoveP1Incidents(IList<IncidentReportModel> firstPriorityIncidents, IList<IncidentReportModel> incidents)
        {
            incidents = incidents.Where(t => !firstPriorityIncidents.Any(x => x.IncidentId == t.IncidentId)).ToList();
            return incidents;
        }

        private async Task<string> IncidentDailyReportGeneration(DateTime yesterday)
        {
            var model = new IncidentDailyReportViewModel()
            {
                FromDate = yesterday,
                ToDate = yesterday,
                RedmineUrl = redmineSettings.BaseUrl + "/issues/",
                PendingIncidents = await GetPendingIncidentCategoryAsync(),
                P1Incidents = await GetP1IncidentCategoryAsync(yesterday, yesterday)
            };

            var content = await viewRenderService.RenderViewToStringAsync(DailyReportEmailTemplate, model);

            return content;
        }

        private async Task<IList<IncidentGroup>> GetPendingIncidentCategoryAsync()
        {
            var pendingIncidents = await incidentService.GetPendingIncidentDailyAsync();
            IList<IncidentReportModel> pendingIncidentGroupByCategory = pendingIncidents.GroupBy(t => t.CategoryName).Select(grp => grp.First()).ToList();
            IList<IncidentGroup> pendingIncidentGroups = pendingIncidentGroupByCategory.Select(t => new IncidentGroup()
            {
                CategoryName = t.CategoryName,
                Incidents = pendingIncidents.Where(x => x.CategoryName == t.CategoryName).ToList()
            }).ToList();

            return pendingIncidentGroups;
        }

        private async Task<IList<IncidentGroup>> GetP1IncidentCategoryAsync(DateTime fromDate, DateTime toDate)
        {
            var priority1Incidents = await incidentService.GetP1IncidentDailyAsync(fromDate, toDate);
            IList<IncidentReportModel> priority1GroupByCategory = priority1Incidents.GroupBy(t => t.CategoryName).Select(grp => grp.First()).ToList();
            IList<IncidentGroup> priority1IncidentGroups = priority1GroupByCategory.Select(t => new IncidentGroup()
            {
                CategoryName = t.CategoryName,
                Incidents = priority1Incidents.Where(x => x.CategoryName == t.CategoryName).ToList()
            }).ToList();

            return priority1IncidentGroups;
        }

        private bool IsAgingIncident(IncidentReportModel incident, DateTime toDate)
        {
            var isAging = incident.PriorityId switch
            {
                (int)Priorities.P1 => incident.IncidentDate.AddDays(1) < GetResolvedDate(incident, toDate),
                (int)Priorities.P2 => incident.IncidentDate.AddDays(2) < GetResolvedDate(incident, toDate),
                (int)Priorities.P3 => incident.IncidentDate.AddDays(7) < GetResolvedDate(incident, toDate),
                _ => false,
            };
            return isAging;
        }

        private DateTime GetResolvedDate(IncidentReportModel incident, DateTime toDate)
            => (incident.ResolvedDate.Year == 1 || (incident.Status != "Closed" && incident.Status != "Cancelled")) ? toDate : incident.ResolvedDate;

        private bool IsCancelled(IncidentReportModel incident)
            => string.Equals(incident.Status, "Cancelled");

        private bool IsClosedInPeriod(IncidentReportModel incident, DateTime fromDate, DateTime toDate)
        {
            var isClosedInPeriod = false;

            if (string.Equals(incident.Status, "Closed") && incident.ResolvedDate.Year != 1)
            {
                isClosedInPeriod = incident.ResolvedDate >= fromDate && incident.ResolvedDate <= toDate;
            }

            return isClosedInPeriod;
        }
    }
}