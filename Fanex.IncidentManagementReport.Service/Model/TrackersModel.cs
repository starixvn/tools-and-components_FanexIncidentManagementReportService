using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fanex.IncidentManagementReport.Service.Model
{
    public class TrackersModel
    {
        [JsonProperty(PropertyName = "trackers")]
        public List<TrackersViewModel> Trackers { get; set; }
    }
}
