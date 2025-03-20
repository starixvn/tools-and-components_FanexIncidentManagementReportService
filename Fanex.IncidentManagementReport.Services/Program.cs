using Fanex.IncidentManagementReport.Services;
using Fanex.IncidentManagementReport.Services.Core.Extensions;
using Fanex.Logging;
using Fanex.Logging.Sentry;
using Fanex.MailDelivery.Client;
using Sentry;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

LogManager
    .SetDefaultLogCategory(configuration.GetValue<string>("Fanex.Logging:LogCategory"))
    .Use(new List<ILogEngine>
    {
        new SentryLogging(new SentryEngineOptions
        {
            Dsn = new Dsn(configuration.GetValue<string>("Fanex.Logging:SentryEngineOptions:Dsn"))
        })
    });

services.AddRazorPages();
services.AddHealthChecks();
services.AddConfigurations(configuration);
services.RegisterHttpClientService(configuration);
services.RegisterDependencyInjection();
ServiceExtension.Services = services;
MailClientHandler.Start();

var app = builder.Build();
app.UseMiddleware<ErrorHandler>();

app.UseRouting();
app.MapHealthChecks("/whoami");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();