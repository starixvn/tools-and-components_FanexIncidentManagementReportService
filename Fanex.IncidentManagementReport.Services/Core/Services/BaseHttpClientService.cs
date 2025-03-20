namespace Fanex.IncidentManagementReport.Services.Core.Services
{
    using Extensions;
    using Models;
    using Newtonsoft.Json;
    using System.Net.Http.Headers;
    using System.Text;

    public class BaseHttpClientService
    {
        private readonly RedmineApiSettings settings;
        private readonly IHttpClientFactory httpClientFactory;

        public BaseHttpClientService(RedmineApiSettings settings, IHttpClientFactory httpClientFactory)
        {
            this.settings = settings;
            this.httpClientFactory = httpClientFactory;
        }

        private HttpRequestMessage CreateRequest(string endPointPath, HttpMethod method)
        {
            Uri url = new($"{settings.BaseUrl}{endPointPath}");
            var request = new HttpRequestMessage(method, url);
            string plainSecretKey = Utilities.Decrypt(settings.SecretKey);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{settings.AccountId}:{plainSecretKey}")));
            return request;
        }

        public async Task<T> ExecuteAsync<T>(string endpoint)
        {
            var httpClient = httpClientFactory.CreateClient("ProxiedClient");
            using var request = CreateRequest(endpoint, HttpMethod.Get);
            using var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string jsonValue = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(jsonValue);
            return result;
        }
    }
}