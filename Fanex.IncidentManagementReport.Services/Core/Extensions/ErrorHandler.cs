using Fanex.IncidentManagementReport.Services.Core.Services;

namespace Fanex.IncidentManagementReport.Services.Core.Extensions;

public class ErrorHandler(RequestDelegate next, ILogServiceProvider logService)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await logService.ErrorSentryAsync(ex);
        }
    }
}