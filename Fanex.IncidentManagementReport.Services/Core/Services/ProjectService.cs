namespace Fanex.IncidentManagementReport.Services.Core.Services;

using Interfaces;
using Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IncidentManagementReport.Services.Models;

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