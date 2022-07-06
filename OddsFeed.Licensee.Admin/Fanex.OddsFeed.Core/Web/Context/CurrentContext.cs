namespace Fanex.OddsFeed.Core.Web.Context
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Net;
    using System.Web;

    public class CurrentContext : ICurrentContext
    {
        public string IP
        {
            get
            {
                var clientIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(clientIp))
                {
                    var forwardedIps = clientIp.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    clientIp = forwardedIps[forwardedIps.Length - 1];
                }
                else
                {
                    clientIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (clientIp == "::1")
                {
                    clientIp = "127.0.0.1";
                }

                return clientIp;
            }
        }

        public string IP4Address
        {
            get
            {
                var ip = string.Empty;
                foreach (IPAddress ipa in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
                {
                    if (ipa.AddressFamily.ToString().Equals("InterNetwork"))
                    {
                        ip = ipa.ToString();
                        break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(ip))
                {
                    return ip;
                }

                foreach (IPAddress ipa in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (ipa.AddressFamily.ToString().Equals("InterNetwork"))
                    {
                        ip = ipa.ToString();
                        break;
                    }
                }

                return ip?.Trim();
            }
        }

        public string IP6Address
        {
            get
            {
                // try to get IP from proxy (if any)
                var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!string.IsNullOrEmpty(ip))
                {
                    var ipsRange = ip.Split(',');
                    var trueIP = ipsRange[0].Trim();
                }
                else
                {
                    // in case client request is not from any proxy
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                return ip;
            }
        }

        public string DomainName
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            }
        }

        public string FullDomainName
        {
            get
            {
                var variables = HttpContext.Current.Request.ServerVariables;

                var domain = variables["SERVER_NAME"];
                var port = variables["SERVER_PORT"];
                var protocol = HttpContext.Current.Request.IsSecureConnection ? "https://" : "http://";

                var fulldomain = protocol + domain;
                var environment = ConfigurationManager.AppSettings["Environment"];
                if (!environment.Equals("Production", StringComparison.OrdinalIgnoreCase)
                    && !environment.Equals("UAT", StringComparison.OrdinalIgnoreCase)
                    && !environment.Equals("SIT", StringComparison.OrdinalIgnoreCase)
                    && !environment.Equals("Local", StringComparison.OrdinalIgnoreCase))
                {
                    fulldomain = fulldomain + ":" + port;
                }

                fulldomain = fulldomain + (HttpRuntime.AppDomainAppVirtualPath == "/" ? string.Empty : HttpRuntime.AppDomainAppVirtualPath);
                return fulldomain;
            }
        }

        public string BrowserName
        {
            get { return HttpContext.Current.Request.Browser.Browser; }
        }

        public string Platform
        {
            get { return HttpContext.Current.Request.Browser.Platform; }
        }

        public string CountryCode
        {
            get
            {
                var regionInfo = ResolveCountry(HttpContext.Current.Request);

                return regionInfo == null ? null : regionInfo.GeoId.ToString(CultureInfo.InvariantCulture);
            }
        }

        private static CultureInfo ResolveCulture(HttpRequest request)
        {
            var languages = request.UserLanguages;

            if (languages == null || languages.Length == 0)
            {
                return null;
            }

            try
            {
                var language = languages[0].ToLowerInvariant().Trim();

                return CultureInfo.CreateSpecificCulture(language);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        private static RegionInfo ResolveCountry(HttpRequest request)
        {
            var culture = ResolveCulture(request);

            if (culture != null)
            {
                return new RegionInfo(culture.LCID);
            }

            return null;
        }
    }
}