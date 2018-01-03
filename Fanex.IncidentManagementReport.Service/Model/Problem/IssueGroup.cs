namespace Fanex.IncidentManagementReport.Service.Model.Problem
{
    using System.Collections.Generic;
    using System.Linq;

    public class IssueGroup
    {
        public int DisplayOrder { get; set; }

        public IList<Issue> Issues { get; set; }

        public string Name { get; set; }

        public IssueGroup(int displayOrder, IEnumerable<Issue> allIssues, int[] trackerIds, int[] statusIds, string name)
        {
            DisplayOrder = displayOrder;
            Issues = Filter(allIssues, trackerIds, statusIds);
            Name = string.Format(name, Issues.Count);
        }

        public IssueGroup(int displayOrder, IEnumerable<Issue> filteredIssue, string name)
        {
            DisplayOrder = displayOrder;
            Issues = filteredIssue.ToList();
            Name = string.Format(name, Issues.Count);
        }

        public IssueGroup(int displayOrder, IEnumerable<Issue> filteredIssue)
        {
            DisplayOrder = displayOrder;
            Issues = filteredIssue.ToList();
        }

        private static IList<Issue> Filter(IEnumerable<Issue> issues, int[] trackers, int[] statusIds)
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