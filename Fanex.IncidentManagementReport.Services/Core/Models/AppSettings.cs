namespace Fanex.IncidentManagementReport.Services.Core.Models;

public class AppSettings
{
    public int ReportedDateCustomFieldId { get; set; }
    public int ResolvedDateCustomFieldId { get; set; }
    public int KnowErrorCustomFieldId { get; set; }
    public int RequestForChangeCustomFieldId { get; set; }
    public int CustomFieldTimeZone { get; set; }
    public int[] IgnoredProjectIds { get; set; }
}