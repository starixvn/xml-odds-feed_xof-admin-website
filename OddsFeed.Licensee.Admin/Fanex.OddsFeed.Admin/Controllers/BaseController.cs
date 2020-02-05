// --------------------------------------------------------------------------
// <copyright file="BaseController.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Common;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Web;
    using Fanex.OddsFeed.Core.Business;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Model.ValueObject;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web.Context;
    using Fanex.OddsFeed.Core.Web.Session;

    public class BaseController : Controller
    {
        protected const string EmptyString = "";

        protected const int NapAdminId = 1;

        protected const int DefaultMinDate = 2000;

        protected readonly ICurrentContext CurrentContext;

        protected readonly ICurrentSession CurrentSession;

        protected readonly IServiceContext ServiceContext;

        protected readonly IBaseService baseService;

        private ViewFlags _viewFlags;

        public BaseController()
        {
            CurrentContext = new CurrentContext();
            ViewBag.Session = CurrentSession = new CurrentSession();
            ViewBag.CurrentDomain = this.CurrentContext.FullDomainName;
            ServiceContext = new ServiceContext
            {
                UserId = this.CurrentSession.UserId,
                SystemId = this.CurrentSession.SystemId,
                Ip = this.CurrentContext.IP
            };

            baseService = new BaseService();

            ViewFlags = new ViewFlags
            {
                IsReCheckin = NapSettings.Settings.EnableReCheckIn
            };

            ViewBag.MinDate = new DateTime(DefaultMinDate, 1, 1);
            ViewBag.MaxDate = DateTime.Now;

            ViewBag.CurrentTheme = "Default";
            ViewBag.IsDashboard = false;

            var navigationHome = new string[] {
                    "<a class='step' href='",
                    ViewBag.CurrentDomain,
                    "/Dashboard/ShowDashboard'><span class='xicon home'></span>Home</a>"
                };

            ViewBag.NavigationHome = string.Join(null, navigationHome);
        }

        protected ViewFlags ViewFlags
        {
            get
            {
                return _viewFlags;
            }

            set
            {
                ViewBag.ViewFlags = _viewFlags = value;
            }
        }

        /// <summary>
        /// Get Error Message in Resource by ErrorCode.
        /// </summary>
        /// <param name="errCode">Error code.</param>
        /// <returns>Error message.</returns>
        public string GetMessage(int errCode)
        {
            var msg = Resources.Lang.ResourceManager.GetString("Msg" + errCode.ToString());
            return msg ?? errCode.ToString();
        }

        /// <summary>
        /// The method to handle URL when switch licensee in dropdown.
        /// </summary>
        /// <param name="systemId">The system id.</param>
        /// <param name="systemName">The system name.</param>
        /// <returns>The page corresponding to URL.</returns>
        [HttpGet]
        public ActionResult Home(byte systemId = 0, string systemName = "")
        {
            if (systemId != 0 && CurrentSession.SystemId != systemId)
            {
                CurrentSession.SystemId = systemId;
                CurrentSession.SystemName = systemName;
                ServiceContext.SystemId = systemId;
            }

            return CurrentSession.ActionName.ToLower().Contains("edit")
                || CurrentSession.ActionName.ToLower().Contains("reset")
                || CurrentSession.ActionName.ToLower().Contains("assign") 
                    ? RedirectToAction(CurrentSession.ActionName, CurrentSession.ControllerName, new { id = 0 }) 
                    : RedirectToAction(CurrentSession.ActionName, CurrentSession.ControllerName);
        }

        public static string DataForAutoComplete(DataTable dt, string fieldName)
        {
            var builder = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                builder.Append(row[fieldName].ToString());
                builder.Append(",");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Create a JSON-formatted for data source.
        /// </summary>
        /// <param name="dt">The data table as data source.</param>
        /// <param name="valueName">String to retrieve the content of column value-Name from data table.</param>
        /// <param name="labelName">String to retrieve the content of column label-Name from data table.</param>
        /// <returns>Represents a JSON-formatted content.</returns>
        public JsonResult JsonForAutoComplete(DataTable dt, string valueName, string labelName)
        {
            var list = new List<JSONSource>();
            foreach (DataRow row in dt.Rows)
            {
                var data = new JSONSource
                {
                    Value = row[valueName].ToString(),
                    Label = row[labelName].ToString()
                };
                list.Add(data);
            }

            return Json(list);
        }

        #region PermissionRegion

        [HttpGet]
        public ActionResult NoAuthorization()
        {
            return View("~/Views/Home/NoAuthorization.cshtml");
        }

        public bool IsAccountManagerRole()
        {
            return CurrentSession.UserRole == (int)Constant.UserRole.ITAdministrator;
        }

        public bool IsLicenseeAdminRole()
        {
            return CurrentSession.UserRole == (int)Constant.UserRole.Licensee;
        }

        #endregion PermissionRegion

        #region Common

        public static string GetVirtualPath()
        {
            var feedUrlPath = ConfigurationManager.AppSettings["FeedUrlPath"];
            if (!string.IsNullOrEmpty(feedUrlPath))
            {
                feedUrlPath += "/";
            }

            var virtualPathWebService = ConfigurationManager.AppSettings["VirtualPathWebService"];
            if (!string.IsNullOrEmpty(virtualPathWebService))
            {
                virtualPathWebService += "/";
            }

            return virtualPathWebService + feedUrlPath;
        }

        public List<SelectListItem> GenerateDelayTime(IEnumerable<SelectListItem> config)
        {
            var delayTimeList = new List<SelectListItem>();
            if (config.Count() >= 2)
            {
                var cacheInterval = Converter.ToInt(config.Where(m => m.Text == "CacheInterval").ToList()[0].Value, 0);
                var numberOfCacheVersion = Converter.ToInt(config.Where(m => m.Text == "NumberOfCacheVersion").ToList()[0].Value, 0);

                for (int i = cacheInterval; i <= (numberOfCacheVersion - 1) * cacheInterval; i += cacheInterval)
                {
                    delayTimeList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                }
            }

            return delayTimeList;
        }

        public int GetMaxNumberOfIPs()
        {
            var config = baseService.GetSelectList(new GetTypeSettingCriteria { SystemID = 0, Type = "Config" }, false);
            return Converter.ToInt(config.Where(m => m.Text == "MaximumNumberOfIPs").ToList()[0].Value, 0);
        }
        #endregion Common
    }
}