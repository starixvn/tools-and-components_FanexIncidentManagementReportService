using System.Collections.Generic;
using Fanex.IncidentManagementReport.Service.Model;
using RestSharp;

namespace Fanex.IncidentManagementReport.Service.Services
{
    public class StatusService : BaseService, IStatusService
    {
        public IList<StatusViewModel> Status
        {
            get
            {
                var request = new RestRequest(Method.GET);
                request.Resource = "issue_statuses.json";
                request.AddHeader("Content-type", "application/json");
                request.RequestFormat = DataFormat.Json;

                var responeResult = RedmineAPI.Execute<StatusModel>(request);

                return responeResult.IssueStatuses;
            }
        }
    }
}
