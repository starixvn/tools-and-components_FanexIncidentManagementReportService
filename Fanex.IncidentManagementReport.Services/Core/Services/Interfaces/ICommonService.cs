namespace Fanex.IncidentManagementReport.Services.Core.Services.Interfaces;

using Fanex.IncidentManagementReport.Services.Models;

internal interface ICommonService
{
    Task<IList<BaseModel>> GetAllPrioritiesAsync();

    Task<IList<BaseModel>> GetAllStatusesAsync();

    Task<IList<BaseModel>> GetAllTrackersAsync();
}