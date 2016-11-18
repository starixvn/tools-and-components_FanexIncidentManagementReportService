namespace Fanex.IncidentManagementReport.Service.DataAccess
{
    using Fanex.IncidentManagementReport.Service.Core;
    using Newtonsoft.Json;
    using RestSharp;
    using RestSharp.Authenticators;
    using System;

    public class RedmineAPI
    {
        private readonly Uri baseUrl;
        private readonly string accountSid;
        private readonly string secretKey;

        public RedmineAPI(string accountSid, string secretKey, string redmineUrl)
        {
            this.accountSid = accountSid;
            this.secretKey = Utilities.Decrypt(secretKey);
            this.baseUrl = new Uri(redmineUrl);
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = baseUrl;
            client.Authenticator = new HttpBasicAuthenticator(accountSid, secretKey);
            request.AddParameter("AccountSid", accountSid, ParameterType.UrlSegment);
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }

            return JsonConvert.DeserializeObject<T>(response.Content);
        }
    }
}
