using Newtonsoft.Json;

namespace Fanex.IncidentManagementReport.Service.Model
{
    public class PriorityViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

    }
}
