using System.Collections.Generic;
using Fanex.IncidentManagementReport.Service.Model;
using RestSharp;

namespace Fanex.IncidentManagementReport.Service.Services
{
    public class TrackersService : BaseService, ITrackersService
    {
        public IList<TrackersViewModel> Trackers
        {
            get
            {
                var request = new RestRequest(Method.GET);
                request.Resource = "trackers.json";
                request.AddHeader("Content-type", "application/json");
                request.RequestFormat = DataFormat.Json;

                var responeResult = RedmineAPI.Execute<TrackersModel>(request);

                return responeResult.Trackers;
            }
        }
    }
}
