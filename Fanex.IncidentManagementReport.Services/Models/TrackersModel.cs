namespace Fanex.IncidentManagementReport.Services.Models;

using Newtonsoft.Json;

public class TrackersModel
{
    [JsonProperty(PropertyName = "trackers")]
    public List<BaseModel> Trackers { get; set; }
}