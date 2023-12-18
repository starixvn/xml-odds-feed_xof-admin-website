namespace Fanex.OddsFeed.Core.Context
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Http;

    public class CurrentContext : ICurrentContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentContext(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string IP
        {
            get
            {
                var clientIp = httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
                if (!string.IsNullOrEmpty(clientIp))
                {
                    var forwardedIps = clientIp.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    clientIp = forwardedIps.Last().Trim();
                }
                else
                {
                    clientIp = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                }

                if (clientIp.ToString() == "::1")
                {
                    clientIp = "127.0.0.1";
                }

                return clientIp.ToString() == null ? "" : clientIp;
            }
        }

        public string DomainName => httpContextAccessor.HttpContext.Request.Host.Host;
    }
}
