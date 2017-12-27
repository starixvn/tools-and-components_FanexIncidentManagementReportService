namespace Fanex.IncidentManagementReport.Service.Services
{
    using System.Collections.Generic;
    using System.Linq;
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
            var result = _issuesService.GetAllIssues(TrackerEnum.Problem);
            result = result.Where(x => !ProjectIdIgnore.Contains(x.Id)).ToList();

            return result;
        }

        public string GetRedmineUrl()
        {
            return RedmineUrl;
        }
    }
}