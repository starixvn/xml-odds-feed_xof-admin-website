namespace Fanex.OddsFeed.Admin.Controllers
{
    using System;
    using System.Data;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Criteria.Dashboard;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Core.Web.Attribute;

    [IsAuthorize]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController()
            : this(null)
        {
        }

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService ?? new DashboardService();
        }


        //// GET: /Dashboard/
        public ActionResult ShowDashboard()
        {
            ViewBag.IsDashboard = true;
            ViewBag.LastReqName = string.Empty;
            GetAccountStatistics();
            return View();
        }

        #region Account Statistics
        public void GetAccountStatistics()
        {
            var criteria = new GetAccountStatisticsCriteria
            {
                SystemID = currentSession.SystemId
            };

            var ds = _dashboardService.GetAccountStatisticsDataSet(criteria);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                var row = ds.Tables[0].Rows[0];
                var lastRequestName = row[4].ToString();

                ViewBag.TotalAff = row[0].ToString();
                ViewBag.TotalSubAcc = row[1].ToString();
                ViewBag.TotalAffTodayReq = row[2].ToString();
                ViewBag.TotalAffMonthReq = row[3].ToString();
                ViewBag.LastReqName = lastRequestName;
                ViewBag.ShowLastReqName = lastRequestName.Length > 10 ? lastRequestName.Substring(0, 10) + "..." : lastRequestName;
                ViewBag.LastReqTime = row[5].ToString();
                ViewBag.LastReqIP = row[6].ToString();
            }

            ViewBag.UserName = currentSession.SystemName;
        }

        #endregion

        #region Login Information

        [HttpPost]
        public string GetLoginInfo()
        {
            var criteria = new GetLoginInfoCriteria
            {
                UserID = currentSession.UserId,
            };
            criteria.PageIndex++;
            var ds = _dashboardService.GetLoginInfoReturnDataSet(criteria);
            return GenerateLogInInfoTable(ds);
        }

        #endregion

        #region Current Online User List

        [HttpPost]
        public string ShowCurrentOnlineUser(string pageSize, string pageIndex)
        {
            var criteria = new GetCurrentOnlineUserCriteria
            {
                UserID = currentSession.UserId,
                RoleID = currentSession.UserRole,
                SystemID = currentSession.SystemId,
                PageIndex = Convert.ToInt16(pageIndex) + 1,
                PageSize = Convert.ToInt16(pageSize)
            };
            var dr = _dashboardService.GetCurrentOnlineUserReturnDataCollection(criteria);
            return FanexGridHelper.DataForFanexGrid(dr);
        }

        #endregion

        #region Recent Activities

        [HttpPost]
        public string ShowRecentActivities(string pageSize, string pageIndex, int optionActivities = 1)
        {
            var criteria = new GetRecentActivitiesCriteria
            {
                UserID = optionActivities == 1 ? currentSession.UserId : 0,
                RoleID = currentSession.UserRole,
                SystemID = currentSession.SystemId,
                PageIndex = Convert.ToInt16(pageIndex) + 1,
                PageSize = Convert.ToInt16(pageSize)
            };
            var dr = _dashboardService.GetRecentActivitiesReturnDataCollection(criteria);
            return FanexGridHelper.DataForFanexGrid_PreventContainQuote(dr);
        }

        #endregion

        #region Private Methods

        private static string GenerateLogInInfoTable(DataSet ds)
        {
            StringBuilder htmlStr = new StringBuilder("<table id=\"logininfotable\"><tr><th>Last login time</th><th>Last login IP</th></tr>");

            if (ds == null || ds.Tables.Count == 0)
            {
                htmlStr.Append("<tr><td colspan=2>There is no data.</td></tr>");
            }
            else
            {
                var dt = ds.Tables[0];
                var rowCounter = dt.Rows.Count;

                for (int i = 0; i < rowCounter; i++)
                {
                    var htmlElement = new[] {
                        "<tr><td>",
                        dt.Rows[i]["LastLoginTime"] == null ? string.Empty : dt.Rows[i]["LastLoginTime"].ToString(),
                        "</td><td>",
                        dt.Rows[i]["IP"] == null ? string.Empty : dt.Rows[i]["IP"].ToString(),
                        "</td></tr>"
                    };
                    htmlStr.Append(string.Join(null, htmlElement));
                }
            }

            htmlStr.Append("</table>");

            return htmlStr.ToString();
        }

        #endregion
    }
}
