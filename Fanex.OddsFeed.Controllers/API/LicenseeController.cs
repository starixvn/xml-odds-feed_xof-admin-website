namespace Fanex.OddsFeed.Controllers.API
{
    using Fanex.OddsFeed.Core.Attribute;
    using Fanex.OddsFeed.Criteria.Licensee;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Services.Interface;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using NLog;

    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiAuthorize]

    public class LicenseeController
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBaseService baseService;
        

        public LicenseeController(IBaseService baseService, 
            IHttpContextAccessor httpContextAccessor)
        {
            this.baseService = baseService;
            this.httpContextAccessor = httpContextAccessor;
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
            var clientIp = httpContextAccessor.HttpContext?.Request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrEmpty(clientIp))
            {
                var forwardedIps = clientIp?.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                clientIp = forwardedIps?.Last().Trim();
            }
            else
            {
                clientIp = httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            }

            if (clientIp?.ToString() == "::1")
            {
                clientIp = "127.0.0.1";
            }

            return clientIp;
        }

        private void WriteLogDeactiveLicensee(string ip, string licenseeName)
        {
            logger.Info("IP: " + ip +
                   "\nAction: Deactive Licensee" +
                   "\nLicensee Name: " + licenseeName);
        }
    }
}
