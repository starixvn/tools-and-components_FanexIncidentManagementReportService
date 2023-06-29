namespace Fanex.IncidentManagementReport.Services
{
    using Fanex.IncidentManagementReport.Services.Core.Extensions;
    using Fanex.IncidentManagementReport.Services.Core.Models;
    using Fanex.IncidentManagementReport.Services.Core.Services;
    using Fanex.IncidentManagementReport.Services.Core.Services.Interfaces;
    using Fanex.IncidentManagementReport.Services.Services;
    using Fanex.IncidentManagementReport.Services.Services.Interfaces;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using NLog;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
            });

            services.AddControllersWithViews();
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddSingleton(appSettings);

            var nexBotSettings = Configuration.GetSection("RedmineApiSettings").Get<RedmineApiSettings>();
            services.AddHttpClient();

            services.AddSingleton(nexBotSettings);
            RegisterDI(services);
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<ErrorHandler>();
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();
            app.UseSession();

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

            services.AddSingleton<IViewRenderService, ViewRenderService>();
            services.AddSingleton<IIncidentService, IncidentService>();
            services.AddSingleton<IProblemManagementReportService, ProblemManagementReportService>();
            services.AddSingleton<IIncidentReportManagementService, IncidentReportManagementService>();
        }
    }
}