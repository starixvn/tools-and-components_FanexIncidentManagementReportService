namespace Fanex.IncidentManagementReport.Service.Model
{
    using System.Collections.Generic;

    public class IncidentGroup
    {
        public string CategoryName { get; set; }

        public IList<IncidentReportModel> Incidents { get; set; }
    }
}
