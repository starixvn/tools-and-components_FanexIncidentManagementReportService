using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.IncidentManagementReport.Service.Model;


namespace Fanex.IncidentManagementReport.Service.Services
{
    public class IncidentReportService : BaseService, IIncidentReportService
    {
        private IIssuesService _issuesService;
        private IProjectService _projectService;       

        public IncidentReportService() : this(null, null) { }
        public IncidentReportService(IIssuesService issuesService, IProjectService projectService)
        {
            _issuesService = issuesService ?? new IssuesService();
            _projectService = projectService ?? new ProjectService();
        }
        public IList<IncidentReportModel> GetIncidentCreateNewReport(DateTime fromDate, DateTime toDate)
        {
            _issuesService.FromDate = fromDate;
            _issuesService.ToDate = toDate;

            var issues = _issuesService.CreateNewIssue;

            if (issues != null)
            {
                return GenerateIncidentModel(issues);
            }

            return null;
        }

        public IList<IncidentReportModel> GetIncidentClosedReport(DateTime fromDate, DateTime toDate)
        {
            _issuesService.FromDate = fromDate;
            _issuesService.ToDate = toDate;

            var issues = _issuesService.CloseIssue;
            if (issues != null)
            {
                return GenerateIncidentModel(issues);
            }

            return null;
        }

        public IList<IncidentReportModel> GetIncidentPendingReport(DateTime fromDate, DateTime toDate)
        {
            _issuesService.FromDate = fromDate;
            _issuesService.ToDate = toDate;

            var issues = _issuesService.PendingIssue;
            if (issues != null)
            {
                return GenerateIncidentModel(issues);
            }

            return null;
        }
        public IList<IncidentReportModel> GetIncidentP1Report(DateTime fromDate, DateTime toDate)
        {
            _issuesService.FromDate = fromDate;
            _issuesService.ToDate = toDate;

            var issues = _issuesService.P1Issue;
            if (issues != null)
            {
                return GenerateIncidentModel(issues);
            }

            return null;
        }       

        public string GetRedmineUrl()
        {
            return RedmineUrl;
        }

        private IList<IncidentReportModel> GenerateIncidentModel(IList<IssuesViewModel> Issues)
        {
            if (Issues != null)
            {
                var query = Issues.Select(iss => new IncidentReportModel
                {
                    IncidentId = iss.Id,
                    IncidentTitle = iss.Subject,
                    CategoryName = (_projectService.GetParentProjectById(iss.Project.Id) != null ? _projectService.GetParentProjectById(iss.Project.Id).Name + " - " : string.Empty) + iss.Project.Name,
                    Assignee = iss.Assignee != null ? iss.Assignee.Name : string.Empty,
                    IncidentDate = iss.IncidentDate,
                    Creator = iss.Creator.Name,
                    Priority = iss.Priority.Name,
                    Status = iss.Status.Name,
                    UpdatedDate = iss.UpdatedDate,
                    CreatedDate = iss.CreatedDate,
                    ResolvedDate = iss.ResolvedDate.Year == 0001 ? "N/A" : iss.ResolvedDate.ToShortDateString()
                });

                return query.ToList();
            }
            else
                return null;
        }       
    }
}
