using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fanex.IncidentManagementReport.Services.Model
{
    public class ProjectModel
    {
        [JsonProperty(PropertyName = "projects")]
        public List<ProjectViewModel> Projects { get; set; }

        [JsonProperty(PropertyName = "total_count")]
        public int TotalCount { get; set; }

        [JsonProperty(PropertyName = "offset")]
        public int Offset { get; set; }

        [JsonProperty(PropertyName = "limit")]
        public int Limit { get; set; }
    }
}
