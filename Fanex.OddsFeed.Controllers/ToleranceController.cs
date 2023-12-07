namespace Fanex.OddsFeed.Controllers
{
    using System;
    using Fanex.Caching;
    using Fanex.OddsFeed.Core.Context;
    using Fanex.OddsFeed.Core.Session;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Services.Interface;
    using Microsoft.AspNetCore.Mvc;
    using NLog;
    using NLog.Web;
    using ONEbook.UM.Services;
    public class ToleranceController : BaseController
    {
        private readonly ICacheService cacheService;
        private readonly IMaintenanceService maintenanceService;

        public ToleranceController(ICurrentContext currentContext, 
            ICurrentSession currentSession,
            IBaseService baseService,
            ICacheService cacheService,
            IMaintenanceService maintenanceService,
            AppSettings appSettings) : base(currentContext, currentSession, baseService, appSettings)
        {
            this.cacheService = cacheService;
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
            const int timeCacheUM = 10;
            var isUM = false;

            string isUMCache = await cacheService.GetAsync<string>("IsUM");
            if (string.IsNullOrEmpty(isUMCache))
            {
                try
                {
                    var umTime = await maintenanceService.GetUnderMaintenanceTimeAsync();
                    startTime = umTime.From;
                    endTime = umTime.To;
                    isUM = umTime.IsUnderMaintenanceTime;

                    var cacheOptions = new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(timeCacheUM));
                    throw new Exception("test 12 12");

                    await cacheService.SetAsync("IsUM", isUM.ToString(), cacheOptions);
                    await cacheService.SetAsync("StartTime", startTime, cacheOptions);
                    await cacheService.SetAsync("EndTime", endTime, cacheOptions);
                }
                catch (Exception ex)
                {
                    var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
                    logger.Error(ex, ex.Message);
                }
            }
            else
            {
                isUM = Convert.ToBoolean(isUMCache);
                startTime = await cacheService.GetAsync<string>("StartTime");
                endTime = await cacheService.GetAsync<string>("EndTime");
            }

            return new Tuple<bool, string, string>(isUM, startTime, endTime);
        }
    }
}
