namespace Fanex.IncidentManagementReport.Service.Model.Problem
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Newtonsoft.Json;

    public class Issue
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        [JsonProperty("tracker")]
        public Tracker Tracker { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("priority")]
        public Priority Priority { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("assigned_to")]
        public AssignedTo AssignedTo { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("done_ratio")]
        public int DoneRatio { get; set; }

        [JsonProperty("custom_fields")]
        public IList<CustomField> CustomFields { get; set; }

        [JsonProperty("created_on")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty("updated_on")]
        public DateTime UpdatedOn { get; set; }

        [JsonProperty("closed_on")]
        public DateTime? CloseOn { get; set; }

        [JsonProperty("relations")]
        public IList<Relation> Relations { get; set; }

        #region Calculated data

        // 35 - Resolved date
        public DateTime? ResolvedDate
        {
            get
            {
                var data = CustomFields.FirstOrDefault(x => x.Id == 35)?.Value.ToString();
                if (!string.IsNullOrEmpty(data))
                {
                    return DateTime.ParseExact(data, "g", new CultureInfo("fr-FR"));
                }

                return null;
            }
        }

        // 37 - Know error
        public string KnowError
        {
            get
            {
                var data = CustomFields.FirstOrDefault(x => x.Id == 37)?.Value.ToString();
                if (!string.IsNullOrEmpty(data))
                {
                    return data == "1" ? "Yes" : "No";
                }

                return null;
            }
        }

        // 39 - Request for Change
        public string RequestForChange
        {
            get
            {
                return CustomFields.FirstOrDefault(x => x.Id == 39)?.Value.ToString();
            }
        }

        #endregion
    }
}