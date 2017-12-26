namespace Fanex.IncidentManagementReport.Service.Model.Problem
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class RedmineResult
    {
        [JsonProperty("issues")]
        public IList<Issue> Issues { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }
    }
}