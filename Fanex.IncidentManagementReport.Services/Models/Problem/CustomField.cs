namespace Fanex.IncidentManagementReport.Services.Models.Problem;

using Newtonsoft.Json;

public class CustomField
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("multiple")]
    public bool Multiple { get; set; }

    [JsonProperty("value")]
    public object Value { get; set; }
}