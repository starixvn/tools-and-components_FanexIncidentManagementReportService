namespace Fanex.IncidentManagementReport.Services.Model.Problem
{
    using Newtonsoft.Json;

    public class AssignedTo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}