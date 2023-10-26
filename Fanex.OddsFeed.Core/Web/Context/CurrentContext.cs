namespace Fanex.OddsFeed.Core.Web.Context
{
    using System.Globalization;
    using System.Net;
    using System.Net.Sockets;
    using Microsoft.AspNetCore.Http;
    using UAParser;

    public class CurrentContext : ICurrentContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string IP
        {
            get
            {
                var clientIp = _httpContextAccessor?.HttpContext?.Request.Headers["X-Forwarded-For"];
                if (!string.IsNullOrEmpty(clientIp))
                {
                    var forwardedIps = clientIp?.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    clientIp = forwardedIps?.Last().Trim();
                }
                else
                {
                    clientIp = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                }

                if (clientIp?.ToString() == "::1")
                {
                    clientIp = "127.0.0.1";
                }

                return clientIp?.ToString() == null ? "" : clientIp;
            }
        }

        public string IP4Address
        {
            get
            {
                var ip = string.Empty;
                var userHostAddress = _httpContextAccessor?.HttpContext?.Connection.RemoteIpAddress;

                if (userHostAddress?.AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = userHostAddress.ToString();
                }
                else
                {
                    foreach (IPAddress ipa in Dns.GetHostAddresses(Dns.GetHostName()))
                    {
                        if (ipa.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ip = ipa.ToString();
                            break;
                        }
                    }
                }

                return ip.Trim();
            }
        }

        public string IP6Address
        {
            get
            {
                var ip = _httpContextAccessor?.HttpContext?.Request.Headers["X-Forwarded-For"];

                if (string.IsNullOrWhiteSpace(ip))
                {
                    ip = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                }

                return ip;
            }
        }

        public string DomainName
        {
            get
            {
                return _httpContextAccessor.HttpContext.Request.Host.Host;
            }
        }

        public string BrowserName
        {
            get
            {
                var userAgent = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"];
                var uaParser = Parser.GetDefault();
                ClientInfo clientInfo = uaParser.Parse(userAgent);

                return clientInfo.UA.Family;
            }
        }

        public string Platform
        {
            get {
                var userAgent = _httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"].ToString();

                if (userAgent.Contains("Windows"))
                {
                    return "Windows";
                }
                else if (userAgent.Contains("Mac"))
                {
                    return "Mac";
                }
                else if (userAgent.Contains("Linux"))
                {
                    return "Linux";
                }
                else
                {
                    return "Other";
                }
            }
        }

        public string CountryCode
        {
            get
            {
                var regionInfo = ResolveCountry(_httpContextAccessor?.HttpContext.Request);
                return regionInfo?.GeoId.ToString(CultureInfo.InvariantCulture);
            }
        }

        private RegionInfo ResolveCountry(HttpRequest request)
        {
            var culture = ResolveCulture(request);

            if (culture != null)
            {
                return new RegionInfo(culture.Name);
            }

            return null;
        }

        private CultureInfo ResolveCulture(HttpRequest request)
        {
            var languages = request.Headers["Accept-Language"].ToString();

            if (string.IsNullOrEmpty(languages))
            {
                return null;
            }

            try
            {
                var language = languages.Split(',')[0].ToLowerInvariant().Trim();
                return CultureInfo.GetCultureInfo(language);
            }
            catch (CultureNotFoundException)
            {
                return null;
            }
        }
    }
}
