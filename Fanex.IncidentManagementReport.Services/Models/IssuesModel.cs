namespace Fanex.IncidentManagementReport.Services.Model
{
    using Newtonsoft.Json;

    public class IssuesModel
    {
        [JsonProperty(PropertyName = "issues")]
        public List<IssuesViewModel> Issues { get; set; }

        [JsonProperty(PropertyName = "total_count")]
        public int TotalCount { get; set; }

        [JsonProperty(PropertyName = "offset")]
        public int Offset { get; set; }

        [JsonProperty(PropertyName = "limit")]
        public int Limit { get; set; }
    }
}