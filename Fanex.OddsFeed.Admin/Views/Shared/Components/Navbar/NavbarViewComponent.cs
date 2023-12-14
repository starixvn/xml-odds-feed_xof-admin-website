namespace Fanex.OddsFeed.Admin.Views.Shared.Components.Navbar
{
    using Fanex.OddsFeed.Core;
    using Microsoft.AspNetCore.Mvc;

    public class NavbarViewComponent : ViewComponent
    {
        private readonly ISessionService sessionService;
        
        public NavbarViewComponent(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }

        public IViewComponentResult Invoke()
        {
            var routeData = RouteData;
            ViewBag.Controller = routeData.Values["controller"];
            ViewBag.Action = routeData.Values["action"];
            ViewBag.RoleID = sessionService.UserRole;

            sessionService.ControllerName = ViewBag.Controller;
            sessionService.ActionName = ViewBag.Action;
            
            return View();
        }
    }
}
