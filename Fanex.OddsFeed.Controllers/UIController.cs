namespace Fanex.OddsFeed.Controllers
{
    using System.Text;
    using Fanex.OddsFeed.Core.Context;
    using Fanex.OddsFeed.Core.Session;
    using Fanex.OddsFeed.Criteria.Administrator;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Services.Interface;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class UIController : BaseController
    {
        public UIController(ICurrentContext currentContext, 
            ICurrentSession currentSession, 
            IBaseService baseService,
            AppSettings appSettings) : base(currentContext, currentSession, baseService, appSettings)
        {
        }

        public async Task<IActionResult> Header()
        {
            ViewBag.SystemList = await GenerateSystemList();
            return PartialView("_Header");
        }

        public ActionResult GlobalScript()
        {
            return PartialView();
        }

        public PartialViewResult NavBar()
        {
            var routeData = RouteData;
            ViewBag.Controller = routeData.Values["controller"];
            ViewBag.Action = routeData.Values["action"];
            ViewBag.RoleID = currentSession.UserRole;

            currentSession.ControllerName = ViewBag.Controller;
            currentSession.ActionName = ViewBag.Action;

            return PartialView("_Narbar");
        }

        private async Task<IEnumerable<SelectListItem>> GenerateSystemList()
        {
            var systems = await baseService.GetDataAsync<SystemDto>(new GetAllSystemsByUserIdCriteria
            {
                UserRole = currentSession.UserRole,
                UserId = currentSession.UserId
            });

            var strSystemList = new StringBuilder();
            foreach (var system in systems)
            {
                strSystemList.Append(system.SystemId + "," + system.SystemName + ";");
            }

            currentSession.SystemList = strSystemList.ToString();

            return systems.Select(x => new SelectListItem
            {
                Value = x.SystemId.ToString(),
                Text = x.SystemName,
                Selected = x.SystemId == currentSession.SystemId
            }).ToList();
        }
    }
}
