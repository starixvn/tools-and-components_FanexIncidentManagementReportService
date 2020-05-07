namespace Testing
{
    using Fanex.IncidentManagementReport.Service.Core;
    using Fanex.MailDelivery.Client;
    using System;
    using Fanex.Logging;
    using Fanex.Logging.Sentry;
    using Sentry;
    using System.Collections.Generic;
    using System.Configuration;

    public class Program
    {
        public static void Main()
        {
            Utilities.RazorEngineConfig();

            LogManager
            .SetDefaultLogCategory(ConfigurationManager.AppSettings["LogCategory"])
            .Use(new List<ILogEngine>
            {
                new SentryLogging(new SentryEngineOptions
                {
                    Dsn = new Dsn(ConfigurationManager.AppSettings["SentryUrl"])
                })
            });

            MailClientHandler.Start();

            ////var fromDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Monday);
            ////var toDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Friday);

            ////Console.WriteLine("from date: " + fromDate);
            ////Console.WriteLine("from date: " + toDate);

            ////var _service = new UsersService();
            ////var _status = new StatusService();
            ////var _project = new ProjectService();
            ////var users = _service.Users;
            ////var status = _status.Status;

            ////var project = _project.Project;

            // //var result =  Core.Encrypt("1234aa");
            ////var x = Core.Decrypt("nUZQQsteGKGhbvzySirUsw==");
            ////var a = Core.Decrypt("Cn8hdjvyaLd9gmMrw0+XWw==");
            //// Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
