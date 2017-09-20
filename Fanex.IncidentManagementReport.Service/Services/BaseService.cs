namespace Fanex.IncidentManagementReport.Service.Services
{
    using System.Configuration;
    using Fanex.IncidentManagementReport.Service.DataAccess;

    public class BaseService
    {
        private readonly RedmineAPI _redmineAPI;

        public BaseService() : this(null)
        {
        }

        public BaseService(RedmineAPI redmineAPI)
        {
            _redmineAPI = redmineAPI ?? new RedmineAPI(AccountSid, SecretKey, RedmineUrl);
        }

        protected RedmineAPI RedmineAPI
        {
            get { return _redmineAPI; }
        }

        protected string RedmineUrl
        {
            get { return ConfigurationManager.AppSettings["redmine-url"]; }
        }

        protected string AccountSid
        {
            get { return ConfigurationManager.AppSettings["accountSid"]; }
        }

        protected string SecretKey
        {
            get { return ConfigurationManager.AppSettings["secretKey"]; }
        }

        protected int ReportedDateCustomFieldId
        {
            get
            {
                var settingString = ConfigurationManager.AppSettings["ReportedDateCustomFieldId"];
                int result;
                int.TryParse(settingString, out result);
                return result;
            }
        }

        protected int ResolvedDateCustomFieldId
        {
            get
            {
                var settingString = ConfigurationManager.AppSettings["ResolvedDateCustomFieldId"];
                int result;
                int.TryParse(settingString, out result);
                return result;
            }
        }

        protected int CustomFieldTimeZone
        {
            get
            {
                var settingString = ConfigurationManager.AppSettings["CustomFieldTimeZone"];
                int result;
                int.TryParse(settingString, out result);
                return result;
            }
        }
    }
}