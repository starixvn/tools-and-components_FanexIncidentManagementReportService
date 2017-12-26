namespace Fanex.IncidentManagementReport.Service.Services
{
    using System.Collections.Generic;
    using Model.Problem;

    public class ProblemReportService : BaseService, IProblemReportService
    {
        private readonly IIssuesService _issuesService;

        public ProblemReportService() : this(null)
        {
        }

        public ProblemReportService(IIssuesService issuesService)
        {
            _issuesService = issuesService ?? new IssuesService();

            _issuesService.Tracker = TrackerEnum.Problem;
        }

        public int[] ProjectIdIgnore => new[] { 31, 56, 65, 66 };

        public IList<Issue> GetAllProblems()
        {
            return _issuesService.GetAllIssues(TrackerEnum.Problem);
        }

        public string GetRedmineUrl()
        {
            return RedmineUrl;
        }
    }
}