using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fanex.IncidentManagementReport.Service.Model
{
    public class PriorityModel
    {
        [JsonProperty(PropertyName = "issue_priorities")]
        public List<PriorityViewModel> Priorities { get; set; }
    }
}
