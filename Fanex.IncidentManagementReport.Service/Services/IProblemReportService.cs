namespace Fanex.IncidentManagementReport.Service.Services
{
    using System.Collections.Generic;
    using Model.Problem;

    public interface IProblemReportService
    {
        IList<Issue> GetAllProblems();

        Issue GetSingleIssue(string issueId);

        string GetRedmineUrl();
    }
}