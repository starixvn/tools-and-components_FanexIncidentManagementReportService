namespace Fanex.IncidentManagementReport.Services.Models.Problem
{
    using Fanex.IncidentManagementReport.Services.Model.Problem;
    using Newtonsoft.Json;

    public class ProblemIssueReponse
    {
        [JsonProperty(PropertyName = "issues")]
        public List<ProblemIssue> Issues { get; set; }

        [JsonProperty(PropertyName = "total_count")]
        public int TotalCount { get; set; }

        [JsonProperty(PropertyName = "offset")]
        public int Offset { get; set; }

        [JsonProperty(PropertyName = "limit")]
        public int Limit { get; set; }
    }
}