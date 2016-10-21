using System.Collections.Generic;
using Fanex.IncidentManagementReport.Service.Model;
using RestSharp;

namespace Fanex.IncidentManagementReport.Service.Services
{
    public class UsersService : BaseService, IUsersService
    {
        public IList<UsersViewModel> Users
        {
            get
            {
                var request = new RestRequest(Method.GET);
                request.Resource = "users.json";
                request.AddHeader("Content-type", "application/json");
                request.RequestFormat = DataFormat.Json;

                var responeResult = RedmineAPI.Execute<UsersModel>(request);

                return responeResult.Users;
            }
        }
    }
}
