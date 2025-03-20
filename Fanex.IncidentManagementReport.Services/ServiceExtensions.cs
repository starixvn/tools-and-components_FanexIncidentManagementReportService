namespace Fanex.IncidentManagementReport.Services;

using Core.Services;
using Core.Services.Interfaces;
using Fanex.IncidentManagementReport.Services.Core.Models;
using Logging;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Services;
using Services.Interfaces;
using System.Net;

public static class ServiceExtensions
{
    public static void RegisterHttpClientService(this IServiceCollection services, IConfiguration configuration)
    {
        var isUseProxy = configuration.GetValue<bool>("ProxySettings:IsUseProxy");
        var proxyAddress = configuration.GetValue<string>("ProxySettings:ProxyAddress");
        var byPassLocal = configuration.GetValue<bool>("ProxySettings:BypassOnLocal");

        services.AddHttpClient("ProxiedClient").ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            UseProxy = isUseProxy,
            Proxy = new WebProxy(proxyAddress, byPassLocal)
        });
    }

    public static void RegisterDependencyInjection(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        services.AddSingleton<ILogServiceProvider, LogServiceProvider>();
        services.AddSingleton<ILogger, Logger>();
        services.AddSingleton<BaseHttpClientService>();
        services.AddSingleton<IIssueService, IssueService>();
        services.AddSingleton<IProjectService, ProjectService>();
        services.AddSingleton<ICommonService, CommonService>();

        services.AddSingleton<IViewRenderService, ViewRenderService>();
        services.AddSingleton<IIncidentService, IncidentService>();
        services.AddSingleton<IProblemManagementReportService, ProblemManagementReportService>();
        services.AddSingleton<IIncidentReportManagementService, IncidentReportManagementService>();
    }

    public static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
        services.AddSingleton(appSettings);

        var nexBotSettings = configuration.GetSection("RedmineApiSettings").Get<RedmineApiSettings>();
        services.AddSingleton(nexBotSettings);
    }
}