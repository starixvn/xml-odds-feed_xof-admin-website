namespace Fanex.OddsFeed.Admin.Views.Shared.Components.Header
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Fanex.OddsFeed.Core;
    using Fanex.OddsFeed.Criteria.Administrator;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Services.Interface;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class HeaderViewComponent : ViewComponent
    {
        private readonly ISessionService sessionService;
        private readonly IBaseService baseService;

        public HeaderViewComponent(ISessionService sessionService, IBaseService baseService)
        {
            this.sessionService = sessionService;
            this.baseService = baseService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.SystemList = await GenerateSystemList();
            ViewBag.Session = sessionService;
            
            return View();
        }

        private async Task<IEnumerable<SelectListItem>> GenerateSystemList()
        {
            var systems = await baseService.GetDataAsync<SystemDto>(new GetAllSystemsByUserIdCriteria
            {
                UserRole = sessionService.UserRole,
                UserId = sessionService.UserId
            });

            var strSystemList = new StringBuilder();
            foreach (var system in systems)
            {
                strSystemList.Append(system.SystemId + "," + system.SystemName + ";");
            }

            sessionService.SystemList = strSystemList.ToString();

            return systems.Select(x => new SelectListItem
            {
                Value = x.SystemId.ToString(),
                Text = x.SystemName,
                Selected = x.SystemId == sessionService.SystemId
            }).ToList();
        }
    }
}
