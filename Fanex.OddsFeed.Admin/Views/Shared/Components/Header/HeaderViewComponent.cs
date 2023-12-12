namespace Fanex.OddsFeed.Admin.Views.Shared.Components.Header
{
    using System.Text;
    using Fanex.OddsFeed.Core.Session;
    using Fanex.OddsFeed.Criteria.Administrator;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Services.Interface;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class HeaderViewComponent : ViewComponent
    {
        private readonly ICurrentSession currentSession;
        private readonly IBaseService baseService;
        public HeaderViewComponent(ICurrentSession currentSession, IBaseService baseService)
        {
            this.currentSession = currentSession;
            this.baseService = baseService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.SystemList = await GenerateSystemList();
            ViewBag.Session = this.currentSession;
            await Task.CompletedTask;
            return View();
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
