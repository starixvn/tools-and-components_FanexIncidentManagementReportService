namespace Fanex.IncidentManagementReport.Services.Models;

using System.Collections.Generic;
using Newtonsoft.Json;

public class UsersModel
{
    [JsonProperty(PropertyName = "users")]
    public List<UsersViewModel> Users { get; set; }

    [JsonProperty(PropertyName = "total_count")]
    public int TotalCount { get; set; }

    [JsonProperty(PropertyName = "offset")]
    public int Offset { get; set; }

    [JsonProperty(PropertyName = "limit")]
    public int Limit { get; set; }
}