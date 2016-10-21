using System.Collections.Generic;
using Fanex.IncidentManagementReport.Service.Model;
using RestSharp;

namespace Fanex.IncidentManagementReport.Service.Services
{
    public class PriorityService : BaseService, IPriorityService
    {
        public IList<PriorityViewModel> Priorities
        {
            get
            {
                var request = new RestRequest(Method.GET);
                request.Resource = "enumerations/issue_priorities.json";
                request.AddHeader("Content-type", "application/json");
                request.RequestFormat = DataFormat.Json;

                var responeResult = RedmineAPI.Execute<PriorityModel>(request);

                return responeResult.Priorities;
            }
        }
    }
}
