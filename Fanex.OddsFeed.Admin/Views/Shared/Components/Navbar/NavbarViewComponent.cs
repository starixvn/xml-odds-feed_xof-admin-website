namespace Fanex.OddsFeed.Admin.Views.Shared.Components.Navbar
{
    using Fanex.OddsFeed.Services.Interface;
    using Microsoft.AspNetCore.Mvc;
    using Fanex.OddsFeed.Core.Session;

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
