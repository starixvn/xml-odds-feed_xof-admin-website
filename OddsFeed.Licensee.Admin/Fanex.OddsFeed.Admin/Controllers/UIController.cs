namespace Fanex.OddsFeed.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.Caching;
    using Fanex.OddsFeed.Core.Criteria.Administrator;

    public class UIController : BaseController
    {
        private readonly ICache cacheManager;

        public UIController() : this(null)
        {
        }

        public UIController(ICache cacheManager)
        {
            this.cacheManager = cacheManager ?? CacheService.CreateInstance();
        }

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
            var allSystemList = baseService.GetSelectList(new GetAllSystemsByUserIdCriteria
            {
                UserRole = currentSession.UserRole,
                UserId = currentSession.UserId
            });

            var strSystemList = new StringBuilder();
            foreach (var system in allSystemList)
            {
                strSystemList.Append(system.Value + "," + system.Text + ";");
            }

            currentSession.SystemList = strSystemList.ToString();
            var systemList = new List<SelectListItem>();
            var systems = strSystemList.ToString().Split(';');
            foreach (string system in systems)
            {
                if (!string.IsNullOrEmpty(system))
                {
                    var nameId = system.Split(',');

                    var item = new SelectListItem
                    {
                        Value = nameId[0] ?? string.Empty,
                        Text = nameId[1] ?? string.Empty
                    };
                    item.Selected = item.Value == currentSession.SystemId.ToString();

                    systemList.Add(item);
                }
            }

            return systemList;
        }
    }
}