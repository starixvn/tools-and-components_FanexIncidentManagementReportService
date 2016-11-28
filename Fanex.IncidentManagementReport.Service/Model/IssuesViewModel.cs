using System;
using Newtonsoft.Json;

namespace Fanex.IncidentManagementReport.Service.Model
{
    public class IssuesViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "project")]
        public ProjectViewModel Project { get; set; }

        [JsonProperty(PropertyName = "tracker")]
        public TrackersViewModel Tracker { get; set; }

        [JsonProperty(PropertyName = "priority")]
        public PriorityViewModel Priority { get; set; }

        [JsonProperty(PropertyName = "status")]
        public StatusViewModel Status { get; set; }

        [JsonProperty(PropertyName = "author")]
        public UsersViewModel Creator { get; set; }

        [JsonProperty(PropertyName = "assigned_to")]
        public UsersViewModel Assignee { get; set; }

        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "start_date")]
        public DateTime IncidentDate { get; set; }

        [JsonProperty(PropertyName = "updated_on")]
        public DateTime UpdatedDate { get; set; }

        [JsonProperty(PropertyName = "created_on")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "due_date")]
        public DateTime ResolvedDate { get; set; }

        [JsonProperty(PropertyName = "closed_on")]
        public DateTime ClosedDate { get; set; }
    }
}
