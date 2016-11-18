namespace Fanex.IncidentManagementReport.Service.Model
{
    using Newtonsoft.Json;

    public class PriorityViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
