namespace Fanex.IncidentManagementReport.Services.Models;

using Newtonsoft.Json;

public class ProjectViewModel
{
    [JsonProperty(PropertyName = "id")]
    public int Id { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "parent")]
    public ProjectViewModel Parent { get; set; }       
}