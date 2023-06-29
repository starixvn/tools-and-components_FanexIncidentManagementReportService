namespace Fanex.IncidentManagementReport.Services.Core.Services
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Fanex.IncidentManagementReport.Services.Core.Models;
    using Fanex.IncidentManagementReport.Services.Core.Services.Interfaces;
    using Fanex.IncidentManagementReport.Services.Model;

    public class ProjectService : BaseHttpClientService, IProjectService
    {
        public ProjectService(RedmineApiSettings settings, IHttpClientFactory httpClientFactory) : base(settings, httpClientFactory)
        {
        }

        public async Task<IList<ProjectViewModel>> GetAllProjectsAsync()
        {
            var url = "projects.json?limit=100";
            var projectModel = await ExecuteAsync<ProjectModel>(url);
            return projectModel.Projects;
        }

        public async Task<ProjectViewModel> GetParentProjectByIdAsync(int projectId)
        {
            var url = $"projects/{projectId}.json";
            var projectModel = await ExecuteAsync<ProjectViewModel>(url);
            return projectModel.Parent;
        }

        public async Task<ProjectViewModel> GetProjectByIdAsync(int projectId)
        {
            var url = $"projects/{projectId}.json";
            var projectModel = await ExecuteAsync<ProjectViewModel>(url);
            return projectModel;
        }
    }
}