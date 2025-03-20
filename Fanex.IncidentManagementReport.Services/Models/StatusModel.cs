namespace Fanex.IncidentManagementReport.Services.Models;

using Newtonsoft.Json;

public class StatusModel
{
    [JsonProperty(PropertyName = "issue_statuses")]
    public List<BaseModel> IssueStatuses { get; set; }
}