namespace Fanex.OddsFeed.Admin.Views.Shared.Components.Navbar
{
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web.Session;
    using Microsoft.AspNetCore.Mvc;
    public class NavbarViewComponent : ViewComponent
    {
        private readonly ICurrentSession currentSession;
        private readonly IBaseService baseService;
        public NavbarViewComponent(ICurrentSession currentSession, IBaseService baseService)
        {
            this.currentSession = currentSession;
            this.baseService = baseService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var routeData = RouteData;
            ViewBag.Controller = routeData.Values["controller"];
            ViewBag.Action = routeData.Values["action"];
            ViewBag.RoleID = currentSession.UserRole;

            currentSession.ControllerName = ViewBag.Controller;
            currentSession.ActionName = ViewBag.Action;
            await Task.CompletedTask;
            return View();
        }
    }
}
