using System;
using System.Collections.Generic;
using Fanex.IncidentManagementReport.Service.Model;
using RestSharp;

namespace Fanex.IncidentManagementReport.Service.Services
{
    public class IssuesService : BaseService, IIssuesService
    {
        public IList<IssuesViewModel> CreateNewIssue
        {
            get
            {

                var request = new RestRequest(Method.GET);
                request.Resource = "issues.json?start_date=" + DateTime.Now.Date.ToString("yyyy-MM-dd");
                request.AddHeader("Content-type", "application/json");
                request.RequestFormat = DataFormat.Json;

                var responeResult = RedmineAPI.Execute<IssuesModel>(request);

                return responeResult.Issues;
            }
        }

        public IList<IssuesViewModel> ChangedIssue
        {
            get
            {
                var request = new RestRequest(Method.GET);
                request.Resource = "issues.json?updated_on=" + DateTime.Now.Date.ToString("yyyy-MM-dd");
                request.AddHeader("Content-type", "application/json");
                request.RequestFormat = DataFormat.Json;

                var responeResult = RedmineAPI.Execute<IssuesModel>(request);

                return responeResult.Issues;
            }
        }

        public IList<IssuesViewModel> PendingIssue
        {
            get
            {
                var request = new RestRequest(Method.GET);
                request.Resource = "issues.json?status_id=1|4|7";
                request.AddHeader("Content-type", "application/json");
                request.RequestFormat = DataFormat.Json;

                var responeResult = RedmineAPI.Execute<IssuesModel>(request);

                return responeResult.Issues;
            }
        }
    }
}
