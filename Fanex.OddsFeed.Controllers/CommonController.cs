namespace Fanex.OddsFeed.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ONEbook.UM.Services;

    public class CommonController(IMaintenanceService maintenanceService) : Controller
    {
        public async Task<IActionResult> UnderMaintainance()
        {
            var downTime = await maintenanceService.GetUnderMaintenanceTimeAsync();
            ViewBag.UmtFrom = downTime.From;
            ViewBag.UmtTo = downTime.To;

            return View();
        }
    }
}