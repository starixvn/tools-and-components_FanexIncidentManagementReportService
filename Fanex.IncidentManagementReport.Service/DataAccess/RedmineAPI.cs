using System;
using Fanex.IncidentManagementReport.Service.Core;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace Fanex.IncidentManagementReport.Service.DataAccess
{
    public class RedmineAPI
    {
        readonly Uri BaseUrl;
        readonly string _accountSid;
        readonly string _secretKey;

        public RedmineAPI(string accountSid, string secretKey, string redmineUrl)
        {
            _accountSid = accountSid;
            _secretKey = Utilities.Decrypt(secretKey);
            BaseUrl = new Uri(redmineUrl);
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = BaseUrl;
            client.Authenticator = new HttpBasicAuthenticator(_accountSid, _secretKey);
            request.AddParameter("AccountSid", _accountSid, ParameterType.UrlSegment);
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }

            return JsonConvert.DeserializeObject<T>(response.Content);
        }
    }
}
