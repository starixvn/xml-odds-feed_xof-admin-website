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
    using Common;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Web;
    using Fanex.OddsFeed.Core.Business;
    using Fanex.OddsFeed.Core.Model.ValueObject;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web.Context;
    using Fanex.OddsFeed.Core.Web.Session;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// Class BaseController.
    /// </summary>    
    public class BaseController : Controller
    {
        protected const string EmptyString = "";

        protected const int NapAdminId = 1;

        //protected readonly string Environment = NapSettings.Settings.Environment;

        //protected readonly int SiteId = NapSettings.Settings.SiteId;

        protected readonly ICurrentContext CurrentContext;

        protected readonly ICurrentSession CurrentSession;

        protected readonly IServiceContext ServiceContext;

        //private int _issiteLogin = NapSettings.Settings.IsSiteLogin;

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

            ViewFlags = new ViewFlags
            {
                IsReCheckin = NapSettings.Settings.EnableReCheckIn
            };

            ViewBag.MinDate = new DateTime(2000, 1, 1);
            ViewBag.MaxDate = DateTime.Now;

            ViewBag.CurrentTheme = "Default";
            ViewBag.IsDashboard = false;

            var navigationHome = new string[]
                                                {
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
                return ViewBag.ViewFlags;
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

            if (CurrentSession.ActionName.ToLower().Contains("edit")
                || CurrentSession.ActionName.ToLower().Contains("reset")
                || CurrentSession.ActionName.ToLower().Contains("assign"))
            {
                return RedirectToAction(CurrentSession.ActionName, CurrentSession.ControllerName, new { id = 0 });
            }
            else
            {
                return RedirectToAction(CurrentSession.ActionName, CurrentSession.ControllerName);
            }
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

        /// <summary>
        /// Build virtual path for xml URLs.
        /// </summary>
        /// <returns>The virtual path.</returns>
        public static string GetVirtualPath()
        {
            var feedUrlPath = ConfigurationManager.AppSettings["FeedUrlPath"].ToString();
            if (!string.IsNullOrEmpty(feedUrlPath))
            {
                feedUrlPath += "/";
            }

            var virtualPathWebService = ConfigurationManager.AppSettings["VirtualPathWebService"].ToString();
            if (!string.IsNullOrEmpty(virtualPathWebService))
            {
                virtualPathWebService += "/";
            }

            return virtualPathWebService + feedUrlPath;
        }

        /// <summary>
        /// Generate the delay time structure from config value.
        /// </summary>
        /// <param name="config">The config value.  {"CacheInterval", "NumberOfCacheVersion"}. </param>
        /// <returns>The delay time structure.</returns>
        public List<SelectListItem> GenerateDelayTime(IEnumerable<SelectListItem> config)
        {
            var delayTimeList = new List<SelectListItem>();
            if (config.Count() >= 2)
            {
                var cacheInterval = Converter.ToInt(config.Where(m => m.Text == "CacheInterval").ToList()[0].Value, 0);
                var numberOfCacheVersion = Converter.ToInt(config.Where(m => m.Text == "NumberOfCacheVersion").ToList()[0].Value, 0);

                for (int i = 0; i <= (numberOfCacheVersion - 2) * cacheInterval; i += cacheInterval)
                {
                    delayTimeList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                }
            }

            return delayTimeList;
        }

        #endregion
    }
}