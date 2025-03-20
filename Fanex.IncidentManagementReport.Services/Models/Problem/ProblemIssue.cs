namespace Fanex.IncidentManagementReport.Services.Models.Problem;

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ProblemIssue
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

    //36 - Resolved date
    public DateTime? ResolvedDate { get; set; }

    // 35 - Reported date
    public DateTime? ReportedDateTime { get; set; }

    //// 37 - Know error
    public string KnowError { get; set; }

    //{
    //    get
    //    {
    //        var data = CustomFields.FirstOrDefault(x => x.Id == KnowErrorCustomFieldId)?.Value.ToString();
    //        if (!string.IsNullOrEmpty(data))
    //        {
    //            return data == "1" ? "Yes" : "No";
    //        }

    //        return null;
    //    }
    //}

    //// 39 - Request for Change
    public string RequestForChange { get; set; }

    public DateTime LastOccurredDateTime
    {
        get
        {
            return Relations.Max(x => x.ReportedDateTime);
        }
    }

    #endregion Calculated data
}