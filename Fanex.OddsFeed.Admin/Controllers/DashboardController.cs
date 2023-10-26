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
        public DashboardController(ICurrentContext currentContext, 
            ICurrentSession currentSession,
            IHttpContextAccessor httpContextAccessor,
            IBaseService baseService) : base(currentContext, currentSession, baseService)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShowDashboard()
        {
            var test = 0;
            if (currentSession.OnUserId == 0)
            {
                test = 1;
            }
            //ViewBag.IsDashboard = true;
            //ViewBag.UserName = currentSession.SystemName;
            //AccountStatistic data = dashboardService.GetAccountStatistics(currentSession.SystemId);
            //return View(data);
            return View();
        }

    }
}
