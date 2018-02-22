namespace Fanex.IncidentManagementReport.Service.Model.Problem
{
    using System;
    using Newtonsoft.Json;

    public class Relation
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("issue_id")]
        public int IssueId { get; set; }

        [JsonProperty("issue_to_id")]
        public int IssueToId { get; set; }

        [JsonProperty("relation_type")]
        public string RelationType { get; set; }

        [JsonProperty("delay")]
        public object Delay { get; set; }

        public DateTime ReportedDateTime { get; set; }
    }
}