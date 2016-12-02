namespace Fanex.OddsFeed.WebServiceLibrary.Helpers
{
    using System;
    using System.Web;

    public static class RequestHelper
    {
        public static string GetClientIpAddress()
        {
            var clientIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(clientIp))
            {
                clientIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                var forwardedIps = clientIp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                clientIp = forwardedIps[forwardedIps.Length - 1];
            }

            if (string.Equals(clientIp, "::1"))
            {
                clientIp = "127.0.0.1";
            }

            return clientIp;
        }
    }
}
