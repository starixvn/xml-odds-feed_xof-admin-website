// --------------------------------------------------------------------------
// <copyright file="DashboardController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Admin.Controllers
{
    using Fanex.OddsFeed.Core.Criteria.Dashboard;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Core.Web.Attribute;
    using ONEbook.UM.Filters;
    using System;
    using System.Data;
    using System.Web.Mvc;

    /// <summary>
    /// Class DashboardController.
    /// </summary>
    [IsAuthorize]
    [CheckUMFilter]
    public class DashboardController : BaseController
    {
        #region Members

        /// <summary>
        /// The _dashboard service.
        /// </summary>
        private readonly IDashboardService _dashboardService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        public DashboardController()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        /// <param name="dashboardService">The dashboard service.</param>
        public DashboardController(IDashboardService dashboardService = null)
        {
            _dashboardService = dashboardService ?? new DashboardService { ServiceContext = ServiceContext };
        }

        #endregion

        //// GET: /Dashboard/
        public ActionResult ShowDashboard()
        {
            ViewBag.IsDashboard = true;
            ViewBag.LastReqName = string.Empty;
            GetAccountStatistics();
            return View();
        }

        #region Account Statistics
        /// <summary>
        /// Gets the account statistics.
        /// </summary>
        public void GetAccountStatistics()
        {
            var criteria = new GetAccountStatisticsCriteria
            {
                SystemID = CurrentSession.SystemId
            };

            DataSet ds = _dashboardService.GetAccountStatisticsDataSet(criteria);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                string lastRequestName = row[4].ToString();

                ViewBag.TotalAff = row[0].ToString();
                ViewBag.TotalSubAcc = row[1].ToString();
                ViewBag.TotalAffTodayReq = row[2].ToString();
                ViewBag.TotalAffMonthReq = row[3].ToString();
                ViewBag.LastReqName = lastRequestName;
                ViewBag.ShowLastReqName = lastRequestName.Length > 10 ? lastRequestName.Substring(0, 10) + "..." : lastRequestName;
                ViewBag.LastReqTime = row[5].ToString();
                ViewBag.LastReqIP = row[6].ToString();
            }

            ViewBag.UserName = CurrentSession.SystemName;
        }

        #endregion

        #region Login Information

        [HttpPost]
        public string GetLoginInfo()
        {
            var criteria = new GetLoginInfoCriteria
            {
                UserID = CurrentSession.UserId,
            };
            criteria.PageIndex++;
            DataSet ds = _dashboardService.GetLoginInfoReturnDataSet(criteria);
            return GenerateLogInInfoTable(ds);
        }

        #endregion

        #region Current Online User List
        /// <summary>
        /// Shows the current online user.
        /// </summary>
        /// <param name="pageSize">The page size.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <returns>The content of online user.</returns>
        [HttpPost]
        public string ShowCurrentOnlineUser(string pageSize, string pageIndex)
        {
            var criteria = new GetCurrentOnlineUserCriteria
            {
                UserID = CurrentSession.UserId,
                RoleID = CurrentSession.UserRole,
                SystemID = CurrentSession.SystemId
            };
            criteria.PageIndex = Convert.ToInt16(pageIndex) + 1;
            criteria.PageSize = Convert.ToInt16(pageSize);
            var dr = _dashboardService.GetCurrentOnlineUserReturnDataCollection(criteria);
            return FanexGridHelper.DataForFanexGrid(dr);
        }
        #endregion

        #region Recent Activities
        /// <summary>
        /// Shows the recent activities.
        /// </summary>
        /// <param name="pageSize">The page size.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="optionActivities">The option activities.</param>
        /// <returns>The content of recent activities.</returns>
        [HttpPost]
        public string ShowRecentActivities(string pageSize, string pageIndex, int optionActivities = 1)
        {
            var criteria = new GetRecentActivitiesCriteria
            {
                UserID = optionActivities == 1 ? CurrentSession.UserId : 0,
                RoleID = CurrentSession.UserRole,
                SystemID = CurrentSession.SystemId
            };
            criteria.PageIndex = Convert.ToInt16(pageIndex) + 1;
            criteria.PageSize = Convert.ToInt16(pageSize);
            var dr = _dashboardService.GetRecentActivitiesReturnDataCollection(criteria);
            return FanexGridHelper.DataForFanexGrid_PreventContainQuote(dr);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Generates the log in information table.
        /// </summary>
        /// <param name="ds">The data set.</param>
        /// <returns>The content of log information table.</returns>
        private string GenerateLogInInfoTable(DataSet ds)
        {
            string htmlStr = "<table id=\"logininfotable\"><tr><th>Last login time</th><th>Last login IP</th></tr>";

            if (ds == null || ds.Tables.Count == 0)
            {
                htmlStr += "<tr><td colspan=2>There is no data.</td></tr>";
            }
            else
            {
                DataTable dt = ds.Tables[0];
                int rowCounter = dt.Rows.Count;

                for (int i = 0; i < rowCounter; i++)
                {
                    string[] htmlElement = new string[]
                                                    {
                                                        "<tr><td>",
                                                        dt.Rows[i]["LastLoginTime"] == null ? string.Empty : dt.Rows[i]["LastLoginTime"].ToString(),
                                                        "</td><td>",
                                                        dt.Rows[i]["IP"] == null ? string.Empty : dt.Rows[i]["IP"].ToString(),
                                                        "</td></tr>"
                                                    };
                    htmlStr += string.Join(null, htmlElement);
                }
            }

            htmlStr += "</table>";
            return htmlStr;
        }

        #endregion
    }
}
