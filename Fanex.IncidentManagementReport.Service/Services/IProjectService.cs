using System.Collections.Generic;
using Fanex.IncidentManagementReport.Service.Model;

namespace Fanex.IncidentManagementReport.Service.Services
{
    public interface IProjectService
    {
        IList<ProjectViewModel> Project { get; }

        ProjectViewModel GetParentProjectById(int projectId);

        ProjectViewModel GetProjectById(int projectId);
    }
}
