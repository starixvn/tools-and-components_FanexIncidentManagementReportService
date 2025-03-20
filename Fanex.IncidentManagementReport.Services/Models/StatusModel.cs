namespace Fanex.IncidentManagementReport.Services.Model
{
    using Models;
    using Newtonsoft.Json;

    public class StatusModel
    {
        [JsonProperty(PropertyName = "issue_statuses")]
        public List<BaseModel> IssueStatuses { get; set; }
    }
}