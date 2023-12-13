namespace Fanex.OddsFeed.Controllers
{
    using System;
    using Fanex.OddsFeed.Core;
    using Fanex.OddsFeed.Core.Context;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Services.Interface;
    using Microsoft.AspNetCore.Mvc;
    using NLog;
    using NLog.Web;
    using ONEbook.UM.Services;
    public class ToleranceController : BaseController
    {
        private readonly IMaintenanceService maintenanceService;

        public ToleranceController(ICurrentContext currentContext, 
            ISessionService sessionService,
            IBaseService baseService,
            IMaintenanceService maintenanceService,
            AppSettings appSettings) : base(currentContext, sessionService, baseService, appSettings)
        {
            this.maintenanceService = maintenanceService;
        }

        public IActionResult Error404()
        {
            return View();
        }

        public IActionResult UnexpectedError()
        {
            return View();
        }

        public IActionResult UnderMaintainance(string timeFrom = "", string timeTo = "")
        {
            ViewBag.UmtFrom = timeFrom;
            ViewBag.UmtTo = timeTo;

            return View();
        }

        public async Task<bool> IsUnderMaintainance()
        {
            var result = await GetDownTime();
            return result.Item1;
        }

        private async Task<Tuple<bool, string, string>> GetDownTime()
        {
            var startTime = string.Empty;
            var endTime = string.Empty;
            var isUM = false;

            try
            {
                var umTime = await maintenanceService.GetUnderMaintenanceTimeAsync();
                startTime = umTime.From;
                endTime = umTime.To;
                isUM = umTime.IsUnderMaintenanceTime;
            }
            catch (Exception ex)
            {
                var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
                logger.Error(ex, ex.Message);
            }

            return new Tuple<bool, string, string>(isUM, startTime, endTime);
        }
    }
}
