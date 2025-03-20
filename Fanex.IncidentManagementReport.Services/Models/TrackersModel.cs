namespace Fanex.IncidentManagementReport.Services.Model
{
    using Models;
    using Newtonsoft.Json;

    public class TrackersModel
    {
        [JsonProperty(PropertyName = "trackers")]
        public List<BaseModel> Trackers { get; set; }
    }
}