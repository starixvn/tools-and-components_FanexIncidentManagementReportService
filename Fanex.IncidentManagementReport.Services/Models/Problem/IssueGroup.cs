namespace Fanex.IncidentManagementReport.Services.Model.Problem
{
    using System.Collections.Generic;
    using System.Linq;

    public class IssueGroup
    {
        public int DisplayOrder { get; set; }

        public IList<ProblemIssue> Issues { get; set; }

        public string Name { get; set; }

        public IssueGroup(int displayOrder, IEnumerable<ProblemIssue> allIssues, int[] trackerIds, int[] statusIds, string name)
        {
            DisplayOrder = displayOrder;
            Issues = Filter(allIssues, trackerIds, statusIds);
            Name = string.Format(name, Issues.Count);
        }

        public IssueGroup(int displayOrder, IEnumerable<ProblemIssue> filteredIssue, string name)
        {
            DisplayOrder = displayOrder;
            Issues = filteredIssue.ToList();
            Name = string.Format(name, Issues.Count);
        }

        public IssueGroup(int displayOrder, IEnumerable<ProblemIssue> filteredIssue)
        {
            DisplayOrder = displayOrder;
            Issues = filteredIssue.ToList();
        }

        private static IList<ProblemIssue> Filter(IEnumerable<ProblemIssue> issues, int[] trackers, int[] statusIds)
        {
            var filteredIssues = issues;

            if (trackers.Any())
            {
                filteredIssues = issues.Where(x => trackers.Contains(x.Tracker.Id));
            }

            if (statusIds.Any())
            {
                filteredIssues = filteredIssues.Where(x => statusIds.Contains(x.Status.Id));
            }

            return filteredIssues.ToList();
        }
    }
}