namespace Fanex.OddsFeed.Controllers.API
{
    using System;
    using System.Threading.Tasks;
    using Fanex.OddsFeed.Core.Attribute;
    using Fanex.OddsFeed.Criteria.Licensee;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiAuthorize]

    public class LicenseeController
    {
        private readonly ILogger<LicenseeController> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBaseService baseService;

        public LicenseeController(IBaseService baseService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<LicenseeController> logger)
        {
            this.baseService = baseService;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Deactive(string name)
        {
            WriteLogDeactiveLicensee(GetClientIP(), name);
            var criteria = new DeactiveLicenseeByNameCriteria
            {
                LicenseeName = name
            };

            CommandResult result = await baseService.ExecuteNonQueryAsync(criteria);
            return new JsonResult(result);
        }

        private string GetClientIP()
        {
            string clientIp = httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrEmpty(clientIp))
            {
                var forwardedIps = clientIp?.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                clientIp = forwardedIps?[forwardedIps.Length - 1].Trim();
            }
            else
            {
                clientIp = httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            }

            if (clientIp == "::1")
            {
                clientIp = "127.0.0.1";
            }

            return clientIp;
        }

        private void WriteLogDeactiveLicensee(string ip, string licenseeName)
        {
            logger.LogInformation("IP: " + ip +
                   "\nAction: Deactive Licensee" +
                   "\nLicensee Name: " + licenseeName);
        }
    }
}
