namespace Fanex.OddsFeed.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Fanex.OddsFeed.Core;
	using Fanex.OddsFeed.Core.Context;
	using Fanex.OddsFeed.Criteria.Affiliate;
	using Fanex.OddsFeed.Models;
	using Fanex.OddsFeed.Resources;
	using Fanex.OddsFeed.Services.Interface;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Rendering;
	using ONEbook.UM.AspNetCore.Filters;

	[ServiceFilter(typeof(CheckUMFilterAttribute))]
	public class BaseController : Controller
	{
		protected const int DefaultMinDate = 2000;

		protected readonly ICurrentContext currentContext;
		protected readonly ISessionService sessionService;
		protected readonly IBaseService baseService;
		private readonly AppSettings appSettings;

		public BaseController(
			ICurrentContext currentContext,
			ISessionService sessionService,
			IBaseService baseService,
			AppSettings appSettings)
		{
			this.currentContext = currentContext;
			this.sessionService = sessionService;
			this.sessionService.LoginIp = this.currentContext.IP;
			this.baseService = baseService;

			ViewBag.Session = this.sessionService;
			ViewBag.MinDate = new DateTimeOffset(DefaultMinDate, 1, 1, 0, 0, 0, TimeSpan.FromHours(-7)).DateTime;
			ViewBag.MaxDate = DateTimeOffset.Now.DateTime;
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
			if (systemId != 0 && sessionService.SystemId != systemId)
			{
				sessionService.SystemId = systemId;
				sessionService.SystemName = systemName;
			}

			return sessionService.ActionName.ToLowerInvariant().Contains("edit")
				|| sessionService.ActionName.ToLowerInvariant().Contains("reset")
				|| sessionService.ActionName.ToLowerInvariant().Contains("assign")
					? RedirectToAction(sessionService.ActionName, sessionService.ControllerName, new { id = 0 })
					: RedirectToAction(sessionService.ActionName, sessionService.ControllerName);
		}

		#region PermissionRegion

		[HttpGet]
		public ActionResult NoAuthorization()
		{
			return View("~/Views/Home/NoAuthorization.cshtml");
		}

		public bool IsAccountManagerRole()
		{
			return sessionService.UserRole == (byte)Core.Enums.UserRole.ITAdministrator;
		}

		public bool IsLicenseeAdminRole()
		{
			return sessionService.UserRole == (byte)Core.Enums.UserRole.Licensee;
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

		public async Task<IEnumerable<SelectListItem>> GenerateDelayTimeAsync()
		{
			var configs = await baseService.GetDataAsync<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" });
			var delayTimeList = new List<SelectListItem>();
			if (configs.Count() > 1)
			{
				var cacheInterval = int.Parse(configs.First(m => m.Key == "CacheInterval").Value);
				var numberOfCacheVersion = int.Parse(configs.First(m => m.Key == "NumberOfCacheVersion").Value);

				for (int i = cacheInterval; i <= (numberOfCacheVersion - 1) * cacheInterval; i += cacheInterval)
				{
					delayTimeList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
				}
			}

			return delayTimeList;
		}

		public async Task<int> GetMaxNumberOfIPsAsync()
		{
			var configs = await baseService.GetDataAsync<SystemSetting>(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" });
			return int.Parse(configs.First(m => m.Key == "MaximumNumberOfIPs").Value);
		}

		#endregion Common
	}
}