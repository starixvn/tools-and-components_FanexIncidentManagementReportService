using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fanex.IncidentManagementReport.Service.Model
{
    public class StatusModel
    {
        [JsonProperty(PropertyName = "issue_statuses")]
        public List<StatusViewModel> IssueStatuses { get; set; }
    }
}
