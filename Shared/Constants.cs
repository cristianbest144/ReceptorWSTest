using System.Configuration;

namespace Shared
{
    public enum LogLevel
    {
        ERROR,
        WARN,
        INFO,
        DEBUG
    }
    public static class Constants 
    {
        public static class LogIndex
        {
            public const string TRAMSACTION_INPROCESS = "TRAMSACTION_INPROCESS";
            public const string TRAMSACTION_ERROR = "TRAMSACTION_ERROR";
            public const string TRAMSACTION_OK = "TRAMSACTION_OK";
            public const string TRAMSACTION_ACTIVE = "TRAMSACTION_ACTIVE";
            public const string LOGS = "LOGS";
        }

        public const string WS_SIESA_CONEXION = "QDM.Siesa.WebService.ConexionWs";
        public const string WS_USER_SIESA = "QDM.Siesa.WebService.UserWs";
        public const string WS_PASS_SIESA = "QDM.Siesa.WebService.PassWs";
        public const string URLSIESAREQUEST = "QDM.Siesa.WebService.Url";
        public const string ACTIONSIESAREQUEST = "QDM.Siesa.WebService.Import";
        public const string PATHSENDSIESAPLANE_PROD = "C:\\Temp\\Siesa";
        //public const string PATHSENDSIESAPLANE_PROD = "wwwroot\\Files\\Siesa\\";
        public const string PATHSENDSIESAPLANE_DEV = "C:\\Temp\\Siesa";
        public const string PARAMETEROPTION = "Siesa-Inlog";
        public const string ENV = "System.Process.Env";
        public static readonly  string COMPANYID = $"System.Process.{ConfigurationSettings.AppSettings[ENV]}.CompanyId";
        public static readonly string SIESACODE = $"System.Process.{ConfigurationSettings.AppSettings[ENV]}.SiesaCode";
        public static readonly string OTHER1 = $"System.Process.{ConfigurationSettings.AppSettings[ENV]}.Other1";
        public static readonly string OTHER2 = $"System.Process.{ConfigurationSettings.AppSettings[ENV]}.Other2";
        public static readonly string OTHER3 = $"System.Process.{ConfigurationSettings.AppSettings[ENV]}.Other3";
        public static readonly string CONNECTIONSTRINGNAME = $"ConnectionStringName.{ConfigurationSettings.AppSettings[ENV]}";
    }

    public static class SettingsEnv
    {
        public const string DEV = "DEV";
        public const string PROD = "PROD";
        public static string ProcessEnv = ConfigurationSettings.AppSettings[Constants.ENV].ToString(); 
    }
}
