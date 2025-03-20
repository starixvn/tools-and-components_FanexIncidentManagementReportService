namespace Fanex.IncidentManagementReport.Services.Models;

using Newtonsoft.Json;

public class UsersViewModel
{
    [JsonProperty(PropertyName = "id")]
    public int Id { get; set; }

    [JsonProperty(PropertyName = "login")]
    public string Login { get; set; }

    [JsonProperty(PropertyName = "firstname")]
    public string FirstName { get; set; }

    [JsonProperty(PropertyName = "lastname")]
    public string LastName { get; set; }

    [JsonProperty(PropertyName = "mail")]
    public string Email { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }
}