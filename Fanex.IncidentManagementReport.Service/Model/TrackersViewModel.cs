using Newtonsoft.Json;

namespace Fanex.IncidentManagementReport.Service.Model
{
    public class TrackersViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
