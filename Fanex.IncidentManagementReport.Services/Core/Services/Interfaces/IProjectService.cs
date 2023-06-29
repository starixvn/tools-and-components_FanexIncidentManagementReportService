namespace Fanex.IncidentManagementReport.Services.Core.Services.Interfaces
{
    using Fanex.IncidentManagementReport.Services.Model;

    public interface IProjectService
    {
        Task<IList<ProjectViewModel>> GetAllProjectsAsync();

        Task<ProjectViewModel> GetParentProjectByIdAsync(int projectId);

        Task<ProjectViewModel> GetProjectByIdAsync(int projectId);
    }
}