using Newtonsoft.Json;

namespace Fanex.IncidentManagementReport.Service.Model
{
    public class ProjectViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "parent")]
        public ProjectViewModel Parent { get; set; }       
    }
}
