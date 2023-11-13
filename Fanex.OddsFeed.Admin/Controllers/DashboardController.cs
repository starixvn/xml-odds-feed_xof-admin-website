using System.Drawing.Printing;
using System.Text;
using Fanex.OddsFeed.Core.Criteria.Dashboard;
using Fanex.OddsFeed.Core.Model;
using Fanex.OddsFeed.Core.Model.Report;
using Fanex.OddsFeed.Core.Service.Interface;
using Fanex.OddsFeed.Core.Web.Attribute;
using Fanex.OddsFeed.Core.Web.Context;
using Fanex.OddsFeed.Core.Web.Session;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.OddsFeed.Admin.Controllers
{
    [IsAuthorize]
    public class DashboardController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDashboardService dashboardService;
        public DashboardController(ICurrentContext currentContext, 
            ICurrentSession currentSession,
            IHttpContextAccessor httpContextAccessor,
            IBaseService baseService,
            IDashboardService dashboardService,
            string feedUrlPath,
            string virtualPathWebService) : base(currentContext, currentSession, baseService, feedUrlPath, virtualPathWebService)
        {
            _httpContextAccessor = httpContextAccessor;
            this.dashboardService = dashboardService;
        }

        public async Task<IActionResult> ShowDashboard()
        {
            ViewBag.IsDashboard = true;
            ViewBag.UserName = currentSession.SystemName;
            AccountStatistic data = await dashboardService.GetAccountStatistics(currentSession.SystemId);
            return View(data);
        }

        [HttpPost]
        public async Task<string> GetLoginInfo()
        {
            var criteria = new GetLoginInfoCriteria
            {
                UserID = currentSession.UserId
            };
            criteria.PageIndex++;
            IEnumerable<LoginInfoStatistic> data = await dashboardService.GetLoginInfo(currentSession.UserId);
            return GenerateLogInInfoTable(data);
        }

        [HttpPost]
        public async Task<IActionResult> ShowCurrentOnlineUser(string pageSize, string pageIndex)
        {
            var criteria = new GetCurrentOnlineUserCriteria
            {
                UserID = currentSession.UserId,
                RoleID = currentSession.UserRole,
                SystemID = currentSession.SystemId,
                PageIndex = Convert.ToInt16(pageIndex) + 1,
                PageSize = Convert.ToInt16(pageSize)
            };
            var result = await baseService.GetDataGrid<UserOnlineStatistic>(criteria);
            return new Utility.CustomJsonResult(result)
            {
                ContentType = "application/json",
            };
        }

        [HttpPost]
        public async Task<IActionResult> ShowRecentActivities(string pageSize, string pageIndex, int optionActivities = 1)
        {
            var criteria = new GetRecentActivitiesCriteria
            {
                UserID = optionActivities == 1 ? currentSession.UserId : 0,
                RoleID = currentSession.UserRole,
                SystemID = currentSession.SystemId,
                PageIndex = Convert.ToInt16(pageIndex) + 1,
                PageSize = Convert.ToInt16(pageSize)
            };
            var result = await baseService.GetDataGrid<RecentActivityStatistic>(criteria);
            return new Utility.CustomJsonResult(result)
            {
                ContentType = "application/json",
            };
        }

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
    }
}
