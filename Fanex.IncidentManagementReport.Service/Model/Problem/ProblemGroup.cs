namespace Fanex.IncidentManagementReport.Service.Model
{
    using System.Collections.Generic;

    public class ProblemGroup
    {
        public string CategoryName { get; set; }

        public IList<ProblemReportModel> Problems { get; set; }
    }
}