namespace Fanex.IncidentManagementReport.Service.Model
{
    using System.Collections.Generic;
    using System.Linq;
    using Core;

    public class ProblemPrioritySummary
    {
        public ProblemPrioritySummary(IList<ProblemReportModel> problems)
        {
            if (problems != null && problems.Any())
            {
                P1Count = problems.Count(t => t.PriorityId == (int)Priorities.P1);
                P2Count = problems.Count(t => t.PriorityId == (int)Priorities.P2);
                P3Count = problems.Count(t => t.PriorityId == (int)Priorities.P3);
            }
        }

        public int P1Count { get; set; }

        public int P2Count { get; set; }

        public int P3Count { get; set; }
    }
}