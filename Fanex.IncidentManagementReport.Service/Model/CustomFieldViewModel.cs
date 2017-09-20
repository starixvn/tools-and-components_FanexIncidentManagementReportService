namespace Fanex.IncidentManagementReport.Service.Model
{
    using Newtonsoft.Json;

    public class CustomFieldViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("multiple")]
        public bool Multiple { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }
    }
}
