namespace Fanex.IncidentManagementReport.Service.Services
{
    using System.Configuration;
    using Fanex.IncidentManagementReport.Service.DataAccess;

    public class BaseService
    {      
        private RedmineAPI _redmineAPI;

        public BaseService() : this(null) 
        { 
        }

        public BaseService(RedmineAPI redmineAPI)
        {
            _redmineAPI = redmineAPI ?? new RedmineAPI(AccountSid, SecretKey, RedmineUrl);
        }

        protected RedmineAPI RedmineAPI
        {
            get
            {
                return _redmineAPI;
            }
        }

        protected string RedmineUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["redmine-url"];
            }
        }

        protected string AccountSid
        {
            get
            {
                return ConfigurationManager.AppSettings["accountSid"];
            }
        }

        protected string SecretKey
        {
            get
            {
                return ConfigurationManager.AppSettings["secretKey"];
            }
        }
    }
}
