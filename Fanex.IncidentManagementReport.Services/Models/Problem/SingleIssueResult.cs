namespace Fanex.IncidentManagementReport.Services.Models.Problem;

using Newtonsoft.Json;

public class SingleIssueResult
{
    [JsonProperty("issue")]
    public ProblemIssue Issue { get; set; }
}