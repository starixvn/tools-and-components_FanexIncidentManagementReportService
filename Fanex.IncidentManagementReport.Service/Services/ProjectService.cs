using System.Collections.Generic;
using System.Linq;
using Fanex.IncidentManagementReport.Service.Model;
using RestSharp;

namespace Fanex.IncidentManagementReport.Service.Services
{
    public class ProjectService : BaseService, IProjectService
    {     

        public IList<ProjectViewModel> Project
        {
            get
            {
                var request = new RestRequest(Method.GET);
                request.Resource = "projects.json?limit=100";
                request.AddHeader("Content-type", "application/json");
                request.RequestFormat = DataFormat.Json;

                var responeResult = RedmineAPI.Execute<ProjectModel>(request);                

                return responeResult.Projects;
            }
        }

        public ProjectViewModel GetParentProjectById(int projectId)
        {
            var project = this.Project.Where(p => p.Id == projectId);

            if (project != null)
            {
                var parentProject = project.First().Parent;
                if(parentProject != null)
                {
                    return parentProject;
                }
            }
                            
            return null;
        }

        public ProjectViewModel GetProjectById(int projectId)
        {
            var project = this.Project.Where(p => p.Id == projectId);

            if (project != null)
                return project.First();
            return null;
        }
    }
}
