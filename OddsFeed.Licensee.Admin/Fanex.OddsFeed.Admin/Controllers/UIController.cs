namespace Fanex.OddsFeed.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Criteria.Administrator;
    using Fanex.OddsFeed.Core.Model;

    public class UIController : BaseController
    {
        public ActionResult Header()
        {
            ViewBag.SystemList = GenerateSystemList();
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult GlobalScript()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult NavBar()
        {
            var routeData = ControllerContext.ParentActionViewContext.RouteData;
            ViewBag.Controller = routeData.Values["controller"];
            ViewBag.Action = routeData.Values["action"];
            ViewBag.RoleID = currentSession.UserRole;

            currentSession.ControllerName = ViewBag.Controller;
            currentSession.ActionName = ViewBag.Action;

            return PartialView();
        }

        private IList<SelectListItem> GenerateSystemList()
        {
            var systems = baseService.GetData<SystemDto>(new GetAllSystemsByUserIdCriteria
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