namespace Fanex.IncidentManagementReport.Service.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.IncidentManagementReport.Service.Model;
    using RestSharp;

    public class IssuesService : BaseService, IIssuesService
    {
        private DateTime _fromDate, _toDate;

        public IList<IssuesViewModel> CreateNewIssue
        {
            get
            {
                var request = new RestRequest(Method.GET);

                request.Resource = "issues.json?start_date=><" + FromDate.ToString("yyyy-MM-dd") + "|" + ToDate.ToString("yyyy-MM-dd") + "&status_id=*&limit=100";
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
                request.Resource = "issues.json?start_date<=" + ToDate.ToString("yyyy-MM-dd") 
                    + "&due_date=><" + FromDate.ToString("yyyy-MM-dd") + "|" + ToDate.ToString("yyyy-MM-dd") + "&status_id=5&limit=100";
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
                request.Resource = "issues.json?start_date<=" + ToDate.ToString("yyyy-MM-dd") + "status_id=!5|6&limit=100";
                request.AddHeader("Content-type", "application/json");
                request.RequestFormat = DataFormat.Json;

                var responeResult = RedmineAPI.Execute<IssuesModel>(request);

                return responeResult.Issues;
            }
        }

        public IList<IssuesViewModel> PendingIssueDaily
        {
            get
            {
                var request = new RestRequest(Method.GET);
                request.Resource = "issues.json?status_id=!5|6&priority_id=!1&limit=100";
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

              request.Resource = "issues.json?start_date=><" + FromDate.ToString("yyyy-MM-dd") + "|" + ToDate.ToString("yyyy-MM-dd") + "&status_id=*&priority_id=1&limit=100";
                request.AddHeader("Content-type", "application/json");
                request.RequestFormat = DataFormat.Json;

                var responeResult = RedmineAPI.Execute<IssuesModel>(request);

                return responeResult.Issues;
            }
        }

        public IList<IssuesViewModel> P1IssueDaily
        {
            get
            {
                var request = new RestRequest(Method.GET);

                request.Resource = "issues.json?created_on=><" + FromDate.ToString("yyyy-MM-dd") + "|" + ToDate.ToString("yyyy-MM-dd") 
                    + "&status_id=*&priority_id=1&limit=100";
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
