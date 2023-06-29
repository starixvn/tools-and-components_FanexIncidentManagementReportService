namespace Fanex.IncidentManagementReport.Services.Model.Problem
{
    using Newtonsoft.Json;

    public class SingleIssueResult
    {
        [JsonProperty("issue")]
        public ProblemIssue Issue { get; set; }
    }
}