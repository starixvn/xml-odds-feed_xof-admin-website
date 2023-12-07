namespace Fanex.OddsFeed.Controllers
{
    using System.Text;
    using Fanex.OddsFeed.Core;
    using Fanex.OddsFeed.Core.Attribute;
    using Fanex.OddsFeed.Core.Context;
    using Fanex.OddsFeed.Core.Session;
    using Fanex.OddsFeed.Criteria.Dashboard;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Models.Report;
    using Fanex.OddsFeed.Services.Interface;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

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
            AppSettings appSettings) : base(currentContext, currentSession, baseService, appSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            this.dashboardService = dashboardService;
        }

        public async Task<IActionResult> ShowDashboard()
        {
            ViewBag.IsDashboard = true;
            ViewBag.UserName = currentSession.SystemName;
            AccountStatistic data = await dashboardService.GetAccountStatisticsAsync(currentSession.SystemId);
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
            IEnumerable<LoginInfoStatistic> data = await dashboardService.GetLoginInfoAsync(currentSession.UserId);
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
            var result = await baseService.GetDataGridAsync<UserOnlineStatistic>(criteria);
            return new CustomJsonResult(result);
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
            var result = await baseService.GetDataGridAsync<RecentActivityStatistic>(criteria);
            return new CustomJsonResult(result);
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
