namespace Fanex.IncidentManagementReport.Services.Core.Services;

using IncidentManagementReport.Services.Models;
using Interfaces;
using Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class ProjectService(RedmineApiSettings settings, IHttpClientFactory httpClientFactory)
    : BaseHttpClientService(settings, httpClientFactory), IProjectService
{
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