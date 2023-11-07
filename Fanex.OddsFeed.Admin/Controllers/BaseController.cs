namespace Fanex.OddsFeed.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.OddsFeed.Core;
    using Fanex.OddsFeed.Core.Business;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web.Context;
    using Fanex.OddsFeed.Core.Web.Session;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class BaseController : Controller
    {
        protected const int DefaultMinDate = 2000;

        protected readonly ICurrentContext currentContext;
        protected readonly ICurrentSession currentSession;
        protected readonly IBaseService baseService;

        public BaseController(
            ICurrentContext currentContext,
            ICurrentSession currentSession,
            IBaseService baseService)
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
        }

        public string GetMessage(int errCode)
        {
            var msg = Lang.ResourceManager.GetString("Msg" + errCode.ToString());
            return msg ?? errCode.ToString();
        }

        [HttpGet]
        public ActionResult Home(byte systemId = 0, string systemName = "")
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

        public static string GetVirtualPath()
        {
            var feedUrlPath = Startup.StaticConfig.GetConnectionString("FeedUrlPath");
            if (!string.IsNullOrEmpty(feedUrlPath))
            {
                feedUrlPath += "/";
            }

            var virtualPathWebService = Startup.StaticConfig.GetConnectionString("VirtualPathWebService");
            if (!string.IsNullOrEmpty(virtualPathWebService))
            {
                virtualPathWebService += "/";
            }

            return virtualPathWebService + feedUrlPath;
        }

        public List<SelectListItem> GenerateDelayTime()
        {
            var configs = baseService.GetData<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" });
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

        public int GetMaxNumberOfIPs()
        {
            var configs = baseService.GetData<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" });
            return Converter.ToInt(configs.First(m => m.Key == "MaximumNumberOfIPs").Value);
        }

        #endregion Common
    }
}