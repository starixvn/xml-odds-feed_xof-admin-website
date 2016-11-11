namespace Fanex.OddsFeed.Library.Configurations
{
    using System;
    using System.Configuration;

    public static class AppSettings
    {
        public static string FeedUrlPath
        {
            get
            {
                return ConfigurationManager.AppSettings["FeedUrlPath"].ToString();
            }
        }

        public static string VirtualPathWebService
        {
            get
            {
                return ConfigurationManager.AppSettings["VirtualPathWebService"].ToString();
            }
        }

        public static bool IsShowError
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["IsShowError"]);
            }
        }

        public static string Environment
        {
            get
            {
                return ConfigurationManager.AppSettings["Environment"].ToString();
            }
        }

        public static int CacheTime
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["CacheTime"]);
            }
        }

        public static int TotalVersion
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["TotalVersion"]);
            }
        }

        public static int LiveCacheTime
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["LiveCacheTime"]);
            }
        }

        public static int Interval
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["Interval"]);
            }
        }

        public static string ServerPool
        {
            get
            {
                return ConfigurationManager.AppSettings["ServerPool"].ToString();
            }
        }

        public static bool IsSpLive
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["SP.Live"]);
            }
        }

        public static string SpSuffix
        {
            get
            {
                return ConfigurationManager.AppSettings["SP.Suffix"].ToString();
            }
        }

        public static string SMTPServer
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPServer"].ToString();
            }
        }

        public static string FromMail
        {
            get
            {
                return ConfigurationManager.AppSettings["FromMail"].ToString();
            }
        }

        public static string ToMail
        {
            get
            {
                return ConfigurationManager.AppSettings["ToMail"].ToString();
            }
        }

        public static string CCMail
        {
            get
            {
                return ConfigurationManager.AppSettings["CCMail"].ToString();
            }
        }

        public static string BCCMail
        {
            get
            {
                return ConfigurationManager.AppSettings["BCCMail"].ToString();
            }
        }

        public static string SmtpClientHost
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpClient.Host"].ToString();
            }
        }

        public static bool SmtpClientEnableSsl
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpClient.EnableSsl"]);
            }
        }

        public static int SmtpClientPort
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["SmtpClient.Port"]);
            }
        }

        public static string SmtpClientAccount
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpClient.Account"].ToString();
            }
        }

        public static string SmtpClientPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpClient.Password"].ToString();
            }
        }

        public static string IpTestingUMTime
        {
            get
            {
                return ConfigurationManager.AppSettings["IPTestingUMTime"].ToString();
            }
        }

        public static string SpTestingUMTime
        {
            get
            {
                return ConfigurationManager.AppSettings["SPTestingUMTime"].ToString();
            }
        }

        public static bool IsDisableUMTimeFlag
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["IsDisableUMTimeFlag"]);
            }
        }

        public static int LimitedOptions
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["LimitedOptions"]);
            }
        }

        public static string HighlightSportType
        {
            get
            {
                return ConfigurationManager.AppSettings["HighlightSportType"].ToString();
            }
        }

        public static string HighlightBetType
        {
            get
            {
                return ConfigurationManager.AppSettings["HighlightBetType"].ToString();
            }
        }

        public static string BetTypeSchemaPath
        {
            get
            {
                return ConfigurationManager.AppSettings["BetTypeSchemaPath"].ToString();
            }
        }

        public static string BetTypeSchemaKey
        {
            get
            {
                return ConfigurationManager.AppSettings["BetTypeSchemaKey"].ToString();
            }
        }
    }
}
