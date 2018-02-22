namespace Fanex.IncidentManagementReport.Service.Model.Problem
{
    using Newtonsoft.Json;

    public class SingleIssueResult
    {
        [JsonProperty("issue")]
        public Issue Issue { get; set; }
    }
}