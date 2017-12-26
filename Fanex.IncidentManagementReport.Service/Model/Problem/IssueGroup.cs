namespace Fanex.IncidentManagementReport.Service.Model.Problem
{
    using System;
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

        public IssueGroup(int displayOrder, IEnumerable<Issue> allIssues, DateTime fromDate, DateTime toDate, string name)
        {
            DisplayOrder = displayOrder;
            Issues = Filter(allIssues, fromDate, toDate);
            Name = string.Format(name, fromDate, toDate);
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

        private static IList<Issue> Filter(IEnumerable<Issue> issues, DateTime fromDate, DateTime toDate)
        {
            var filteredIssues = issues;

            filteredIssues = filteredIssues.Where(x => fromDate <= x.CreatedOn && x.CreatedOn <= toDate);

            return filteredIssues.ToList();
        }
    }
}