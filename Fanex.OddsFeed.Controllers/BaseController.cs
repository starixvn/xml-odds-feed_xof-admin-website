namespace Fanex.OddsFeed.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.OddsFeed.Core;
    using Fanex.OddsFeed.Core.Context;
    using Fanex.OddsFeed.Core.Session;
    using Fanex.OddsFeed.Criteria.Affiliate;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Resources;
    using Fanex.OddsFeed.Services.Interface;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class BaseController : Controller
    {
        protected const int DefaultMinDate = 2000;

        protected readonly ICurrentContext currentContext;
        protected readonly ICurrentSession currentSession;
        protected readonly IBaseService baseService;
        private readonly AppSettings appSettings;

        public BaseController(
            ICurrentContext currentContext,
            ICurrentSession currentSession,
            IBaseService baseService,
            AppSettings appSettings)
        {
            this.currentContext = currentContext;
            this.currentSession = currentSession ?? new CurrentSession()
            {
                LoginIp = this.currentContext.IP
            };
            this.baseService = baseService;

            ViewBag.Session = this.currentSession;
            ViewBag.MinDate = new DateTime(DefaultMinDate, 1, 1);
            ViewBag.MaxDate = DateTime.Now;
            ViewBag.CurrentTheme = "Default";
            ViewBag.IsDashboard = false;
            this.appSettings = appSettings;
        }

        public string GetMessage(int errCode)
        {
            var msg = Lang.ResourceManager.GetString("Msg" + errCode.ToString());
            return msg ?? errCode.ToString();
        }

        [HttpGet]
        public ActionResult Home(short systemId = 0, string systemName = "")
        {
            if (systemId != 0 && currentSession.SystemId != systemId)
            {
                currentSession.SystemId = systemId;
                currentSession.SystemName = systemName;
            }

            return currentSession.ActionName.ToLowerInvariant().Contains("edit")
                || currentSession.ActionName.ToLowerInvariant().Contains("reset")
                || currentSession.ActionName.ToLowerInvariant().Contains("assign")
                    ? RedirectToAction(currentSession.ActionName, currentSession.ControllerName, new { id = 0 })
                    : RedirectToAction(currentSession.ActionName, currentSession.ControllerName);
        }

        #region PermissionRegion

        [HttpGet]
        public ActionResult NoAuthorization()
        {
            return View("~/Views/Home/NoAuthorization.cshtml");
        }

        public bool IsAccountManagerRole()
        {
            return currentSession.UserRole == (int)Constant.UserRole.ITAdministrator;
        }

        public bool IsLicenseeAdminRole()
        {
            return currentSession.UserRole == (int)Constant.UserRole.Licensee;
        }

        #endregion PermissionRegion

        #region Common

        public string GetVirtualPath()
        {
            var fUrlPath = appSettings.FeedUrlPath;
            if (!string.IsNullOrEmpty(fUrlPath))
            {
                fUrlPath += "/";
            }

            var vPathWebService = appSettings.VirtualPathWebService;
            if (!string.IsNullOrEmpty(vPathWebService))
            {
                vPathWebService += "/";
            }

            return vPathWebService + fUrlPath;
        }

        public async Task<IEnumerable<SelectListItem>> GenerateDelayTime()
        {
            var configs = await baseService.GetDataAsync<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" });
            var delayTimeList = new List<SelectListItem>();
            if (configs.Count() > 1)
            {
                var cacheInterval = Converter.ToInt(configs.First(m => m.Key == "CacheInterval").Value);
                var numberOfCacheVersion = Converter.ToInt(configs.First(m => m.Key == "NumberOfCacheVersion").Value);

                for (int i = cacheInterval; i <= (numberOfCacheVersion - 1) * cacheInterval; i += cacheInterval)
                {
                    delayTimeList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                }
            }

            return delayTimeList;
        }

        public async Task<int> GetMaxNumberOfIPs()
        {
            var configs = await baseService.GetDataAsync<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" });
            return Converter.ToInt(configs.First(m => m.Key == "MaximumNumberOfIPs").Value);
        }

        #endregion Common
    }
}