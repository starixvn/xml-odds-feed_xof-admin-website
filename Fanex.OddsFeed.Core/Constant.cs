namespace Fanex.OddsFeed.Core
{
    public static class Constant
    {
        public const string SPACE = " ";

        public const string OK = "OK";

        public const string ERROR = "ERROR";

        public const string UAT = "UAT";

        public const string LIVE = "LIVE";

        public const string SiteId = "SiteId";

        public const string Environment = "Environment";

        public const int SearchAllSite = -9999;

        public enum Policy
        {
            None,
            Read,
            Host
        }

        public enum Crud
        {
            Read,
            Host
        }

        public enum Level
        {
            User,
            SiteAdmin = 50,
            PortalAdmin = 100
        }

        public enum UserRole
        {
            ITAdministrator = 1,
            Licensee,
            LicenseeSubAccount
        }
    }
}