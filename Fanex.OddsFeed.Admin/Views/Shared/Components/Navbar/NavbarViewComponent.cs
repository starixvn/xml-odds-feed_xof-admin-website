namespace Fanex.OddsFeed.Admin.Views.Shared.Components.Navbar
{
    using Fanex.OddsFeed.Core;
    using Fanex.OddsFeed.Services.Interface;
    using Microsoft.AspNetCore.Mvc;

    public class NavbarViewComponent : ViewComponent
    {
        private readonly ISessionService sessionService;
        private readonly IBaseService baseService;
        public NavbarViewComponent(ISessionService sessionService, IBaseService baseService)
        {
            this.sessionService = sessionService;
            this.baseService = baseService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var routeData = RouteData;
            ViewBag.Controller = routeData.Values["controller"];
            ViewBag.Action = routeData.Values["action"];
            ViewBag.RoleID = sessionService.UserRole;

            sessionService.ControllerName = ViewBag.Controller;
            sessionService.ActionName = ViewBag.Action;
            await Task.CompletedTask;
            return View();
        }
    }
}
