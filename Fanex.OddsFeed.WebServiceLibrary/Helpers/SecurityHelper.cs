namespace Fanex.OddsFeed.WebServiceLibrary.Helpers
{
    using System.Security;

    public static class SecurityHelper
    {
        public static string EscapeXML(string content)
        {
            return SecurityElement.Escape(content);
        }
    }
}
