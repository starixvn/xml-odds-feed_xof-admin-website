namespace Fanex.OddsFeed.WebService.Controllers
{
    using System;
    using System.Web.Mvc;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;

    public class BaseController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;

        public BaseController()
        {
            _maintenanceService = new MaintenanceService();
        }

        protected string GetIpAddressOfClient()
        {
            var clientIp = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(clientIp))
            {
                var forwardedIps = clientIp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                clientIp = forwardedIps[forwardedIps.Length - 1];
            }
            else
            {
                clientIp = Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.Equals(clientIp, "::1"))
            {
                clientIp = "127.0.0.1";
            }

            return clientIp;
        }

        protected bool IsDownTime()
        {
            return _maintenanceService.IsUnderMaintenance();
        }
    }
}