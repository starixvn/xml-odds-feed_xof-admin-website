namespace Fanex.OddsFeed.Core.Context
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Http;

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

        public string DomainName
        {
            get
            {
                return _httpContextAccessor.HttpContext.Request.Host.Host;
            }
        }
    }
}
