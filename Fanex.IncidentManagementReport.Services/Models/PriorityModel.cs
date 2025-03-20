namespace Fanex.IncidentManagementReport.Services.Models;

using Newtonsoft.Json;

public class PriorityModel
{
    [JsonProperty(PropertyName = "issue_priorities")]
    public List<BaseModel> Priorities { get; set; }
}