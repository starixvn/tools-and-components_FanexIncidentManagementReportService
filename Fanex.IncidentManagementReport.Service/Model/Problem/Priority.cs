namespace Fanex.IncidentManagementReport.Service.Model.Problem
{
    using Newtonsoft.Json;

    public class Priority
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}