namespace Fanex.IncidentManagementReport.Services.Models;

using Newtonsoft.Json;

public class BaseModel
{
    [JsonProperty(PropertyName = "id")]
    public int Id { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }
}