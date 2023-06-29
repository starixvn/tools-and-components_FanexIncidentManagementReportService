namespace Fanex.IncidentManagementReport.Services.Core.Services
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Fanex.IncidentManagementReport.Services.Core.Models;
    using Fanex.IncidentManagementReport.Services.Core.Services.Interfaces;
    using Fanex.IncidentManagementReport.Services.Model;
    using Fanex.IncidentManagementReport.Services.Models;

    public class CommonService : BaseHttpClientService, ICommonService
    {
        public CommonService(RedmineApiSettings settings, IHttpClientFactory httpClientFactory) : base(settings, httpClientFactory)
        {
        }

        public async Task<IList<BaseModel>> GetAllPrioritiesAsync()
        {
            string url = "enumerations/issue_priorities.json";
            var priorityModel = await ExecuteAsync<PriorityModel>(url);
            return priorityModel.Priorities;
        }

        public async Task<IList<BaseModel>> GetAllStatusesAsync()
        {
            string url = "issue_statuses.json";
            var priorityModel = await ExecuteAsync<StatusModel>(url);
            return priorityModel.IssueStatuses;
        }

        public async Task<IList<BaseModel>> GetAllTrackersAsync()
        {
            string url = "trackers.json";
            var priorityModel = await ExecuteAsync<TrackersModel>(url);
            return priorityModel.Trackers;
        }
    }
}