namespace Fanex.IncidentManagementReport.Services.Model.Problem
{
    using Newtonsoft.Json;

    public class Status
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}