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

                request.Resource = "issues.json?start_date=" + FromDate + "|" + ToDate + "&limit=100";
                request.AddHeader("Content-type", "application/json");
                request.RequestFormat = DataFormat.Json;

                var responeResult = RedmineAPI.Execute<IssuesModel>(request);

                return responeResult.Issues;
            }
        }

        public IList<IssuesViewModel> CloseIssue
        {
            get
            {
                var request = new RestRequest(Method.GET);
                request.Resource = "issues.json?updated_on=" + FromDate + "|" + ToDate + "&status_id=5&limit=100"; 
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
                request.Resource = "issues.json?start_date=%3C%3D" + FromDate + "&status_id =1|4|7&limit=100";
                request.AddHeader("Content-type", "application/json");
                request.RequestFormat = DataFormat.Json;

                var responeResult = RedmineAPI.Execute<IssuesModel>(request);

                return responeResult.Issues;
            }
        }

        public IList<IssuesViewModel> P1Issue
        {
            get
            {
                var request = new RestRequest(Method.GET);

                request.Resource = "issues.json?start_date=" + FromDate + "|" + ToDate + "&priority=1&limit=100";
                request.AddHeader("Content-type", "application/json");
                request.RequestFormat = DataFormat.Json;

                var responeResult = RedmineAPI.Execute<IssuesModel>(request);

                return responeResult.Issues;
            }
        }

        public string FromDate
        {
            get
            {
                return DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            }
        }

        public string ToDate
        {
            get
            {
                return DateTime.Now.AddDays(-8).ToString("yyyy-MM-dd");
            }
        }

        public string NowDate
        {
            get
            {
               return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }        
    }
}
