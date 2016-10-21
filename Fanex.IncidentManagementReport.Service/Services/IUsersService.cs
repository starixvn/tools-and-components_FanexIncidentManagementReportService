using System.Collections.Generic;
using Fanex.IncidentManagementReport.Service.Model;

namespace Fanex.IncidentManagementReport.Service.Services
{
    public interface IUsersService
    {
        IList<UsersViewModel> Users { get; }
    }
}
