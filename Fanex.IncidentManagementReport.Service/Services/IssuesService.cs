using System;
using System.Collections.Generic;
using Fanex.IncidentManagementReport.Service.Model;
using RestSharp;

namespace Fanex.IncidentManagementReport.Service.Services
{
    public class IssuesService : BaseService, IIssuesService
    {
        private DateTime _fromDate, _toDate;
        public IList<IssuesViewModel> CreateNewIssue
        {
            get
            {
                var request = new RestRequest(Method.GET);

                request.Resource = "issues.json?start_date=" + FromDate.ToString("yyyy-MM-dd") + "|" + ToDate.ToString("yyyy-MM-dd") + "&limit=100";
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
                request.Resource = "issues.json?updated_on=" + FromDate.ToString("yyyy-MM-dd") + "|" + ToDate.ToString("yyyy-MM-dd") + "&status_id=5&limit=100";
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
                request.Resource = "issues.json?start_date=%3C%3D" + FromDate.ToString("yyyy-MM-dd") + "&status_id =1|4|7&limit=100";
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

                request.Resource = "issues.json?start_date=" + FromDate.ToString("yyyy-MM-dd") + "|" + ToDate.ToString("yyyy-MM-dd") + "&priority=1&limit=100";
                request.AddHeader("Content-type", "application/json");
                request.RequestFormat = DataFormat.Json;

                var responeResult = RedmineAPI.Execute<IssuesModel>(request);

                return responeResult.Issues;
            }
        }

        public DateTime FromDate
        {
            get
            {
                return _fromDate;
            }

            set
            {
                _fromDate = value;
            }
        }

        public DateTime ToDate
        {
            get
            {
                return _toDate;
            }

            set
            {
                _toDate = value;
            }
        }     
    }
}
