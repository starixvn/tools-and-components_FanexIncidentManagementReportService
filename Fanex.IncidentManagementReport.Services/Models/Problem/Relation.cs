namespace Fanex.IncidentManagementReport.Services.Model.Problem
{
    using Core.Enums;
    using Newtonsoft.Json;
    using System;

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

        public int DisplayedId { get; set; }

        public TrackerEnum Tracker { get; set; }
    }
}