namespace Fanex.OddsFeed.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Criteria.Dashboard;
    using Fanex.OddsFeed.Core.Model.Report;
    using Fanex.OddsFeed.Core.Service;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Core.Web.Attribute;

    [IsAuthorize]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService dashboardService;

        public DashboardController() : this(null)
        {
        }

        public DashboardController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService ?? new DashboardService();
        }


        //// GET: /Dashboard/
        public ActionResult ShowDashboard()
        {
            ViewBag.IsDashboard = true;
            ViewBag.UserName = currentSession.SystemName;
            AccountStatistic data = dashboardService.GetAccountStatistics(currentSession.SystemId);
            return View(data);
        }

        #region Login Information

        [HttpPost]
        public string GetLoginInfo()
        {
            var criteria = new GetLoginInfoCriteria
            {
                UserID = currentSession.UserId
            };
            criteria.PageIndex++;
            IEnumerable<LoginInfoStatistic> data = dashboardService.GetLoginInfo(currentSession.UserId);
            return GenerateLogInInfoTable(data);
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
            var dr = baseService.GetData(criteria);
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
            var dr = baseService.GetData(criteria);
            return FanexGridHelper.DataForFanexGrid_PreventContainQuote(dr);
        }

        #endregion

        #region Private Methods

        private static string GenerateLogInInfoTable(IEnumerable<LoginInfoStatistic> records)
        {
            var htmlStr = new StringBuilder("<table id=\"logininfotable\"><tr><th>Last login time</th><th>Last login IP</th></tr>");

            if (records == null || !records.Any())
            {
                htmlStr.Append("<tr><td colspan=2>There is no data.</td></tr>");
            }
            else
            {
                foreach (var record in records)
                {
                    var htmlElement = new[] {
                        "<tr><td>",
                        record.LastLoginTime,
                        "</td><td>",
                        record.IP,
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