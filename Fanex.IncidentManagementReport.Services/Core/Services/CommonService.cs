namespace Fanex.IncidentManagementReport.Services.Core.Services
{
    using Fanex.IncidentManagementReport.Services.Models;
    using Interfaces;
    using Model;
    using Models;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

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