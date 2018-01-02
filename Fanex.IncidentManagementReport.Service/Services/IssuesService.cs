namespace Fanex.IncidentManagementReport.Service.Services
{
    using System;
    using System.Collections.Generic;
    using Model;
    using Model.Problem;
    using RestSharp;

    public class IssuesService : BaseService, IIssuesService
    {
        public IList<IssuesViewModel> CreateNewIssue
        {
            get
            {
                var request = new RestRequest(Method.GET)
                {
                    Resource =
                        $"issues.json?start_date=><{FromDate:yyyy-MM-dd}|{ToDate:yyyy-MM-dd}&status_id=*&limit=100&tracker_id={(int)Tracker}"
                };


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
                var request = new RestRequest(Method.GET)
                {
                    Resource =
                        $"issues.json?start_date<={ToDate:yyyy-MM-dd}&due_date=><{FromDate:yyyy-MM-dd}|{ToDate:yyyy-MM-dd}&status_id=5&limit=100&tracker_id={(int)Tracker}"
                };

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
                var request = new RestRequest(Method.GET)
                {
                    Resource = $"issues.json?start_date<={ToDate:yyyy-MM-dd}status_id=!5|6&limit=100&tracker_id={(int)Tracker}"
                };

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
                var request = new RestRequest(Method.GET)
                {
                    Resource = $"issues.json?status_id=!5|6&priority_id=!1&limit=100&tracker_id={(int)Tracker}"
                };

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
                var request = new RestRequest(Method.GET)
                {
                    Resource =
                        $"issues.json?start_date=><{FromDate:yyyy-MM-dd}|{ToDate:yyyy-MM-dd}&status_id=*&priority_id=1&limit=100&tracker_id={(int)Tracker}"
                };


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
                var request = new RestRequest(Method.GET)
                {
                    Resource =
                        $"issues.json?created_on=><{FromDate:yyyy-MM-dd}|{ToDate:yyyy-MM-dd}&status_id=*&priority_id=1&limit=100&tracker_id={(int)Tracker}"
                };


                request.AddHeader("Content-type", "application/json");
                request.RequestFormat = DataFormat.Json;

                var responeResult = RedmineAPI.Execute<IssuesModel>(request);

                return responeResult.Issues;
            }
        }

        public IList<Issue> GetAllIssues(TrackerEnum tracker)
        {
            var request = new RestRequest(Method.GET)
            {
                Resource =
                    $"issues.json?status_id=*&limit=100&tracker_id={(int)Tracker}&include=relations"
            };

            request.AddHeader("Content-type", "application/json");
            request.RequestFormat = DataFormat.Json;

            return RedmineAPI.Execute<RedmineResult>(request).Issues;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public TrackerEnum Tracker { get; set; }
    }
}