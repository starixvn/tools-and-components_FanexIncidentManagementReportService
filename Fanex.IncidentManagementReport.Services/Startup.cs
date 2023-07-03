namespace Fanex.IncidentManagementReport.Services
{
    using Fanex.IncidentManagementReport.Services.Core.Extensions;
    using Fanex.IncidentManagementReport.Services.Core.Models;
    using Fanex.IncidentManagementReport.Services.Core.Services;
    using Fanex.IncidentManagementReport.Services.Core.Services.Interfaces;
    using Fanex.IncidentManagementReport.Services.Services;
    using Fanex.IncidentManagementReport.Services.Services.Interfaces;
    using Fanex.Logging;
    using Fanex.Logging.Sentry;
    using Fanex.MailDelivery.Client;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Sentry;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            LogManager
                .SetDefaultLogCategory(Configuration.GetValue<string>("Fanex.Logging:LogCategory"))
                .Use(new List<ILogEngine>
                {
                        new SentryLogging(new SentryEngineOptions
                        {
                            Dsn = new Dsn(Configuration.GetValue<string>("Fanex.Logging:SentryEngineOptions:Dsn"))
                        })
                });

            services.AddControllersWithViews();
            services.AddRazorPages();

            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddSingleton(appSettings);

            var nexBotSettings = Configuration.GetSection("RedmineApiSettings").Get<RedmineApiSettings>();
            services.AddSingleton(nexBotSettings);

            services.AddHttpClient();

            RegisterDI(services);
            ServiceExtension.Services = services;
            MailClientHandler.Start();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseMiddleware<ErrorHandler>();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void RegisterDI(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddSingleton<ILogServiceProvider, LogServiceProvider>();
            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<BaseHttpClientService>();
            services.AddSingleton<IIssueService, IssueService>();
            services.AddSingleton<IProjectService, ProjectService>();
            services.AddSingleton<ICommonService, CommonService>();

            services.AddSingleton<IRazorViewEngine, RazorViewEngine>();
            //services.AddSingleton<ITempDataProvider>();
            services.AddSingleton<IViewRenderService, ViewRenderService>();
            services.AddSingleton<IIncidentService, IncidentService>();
            services.AddSingleton<IProblemManagementReportService, ProblemManagementReportService>();
            services.AddSingleton<IIncidentReportManagementService, IncidentReportManagementService>();
        }
    }
}