using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.IncidentManagementReport.Service.Core;
using Fanex.IncidentManagementReport.Service.Services;
using Fanex.MailDelivery.Client.Handlers;
using FluentDate;
using FluentDateTime;

namespace testing
{
    class Program
    {
        static void Main(string[] args)
        {
            Utilities.RazorEngineConfig();
            MailClientHandler.Start();

            //var fromDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Monday);
            //var toDate = Utilities.DateOfPreviousWeek(DateTime.Now, DayOfWeek.Friday);

            //Console.WriteLine("from date: " + fromDate);
            //Console.WriteLine("from date: " + toDate);

            //var _service = new UsersService();
            //var _status = new StatusService();
            //var _project = new ProjectService();
            //var users = _service.Users;
            //var status = _status.Status;

            //var project = _project.Project;

            // var result =  Core.Encrypt("chuotnhat");

            //var a = Core.Decrypt("Cn8hdjvyaLd9gmMrw0+XWw==");
            // Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
