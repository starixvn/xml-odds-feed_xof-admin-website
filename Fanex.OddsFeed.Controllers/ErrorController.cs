namespace Fanex.OddsFeed.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ONEbook.UM.Services;

    public class ErrorController : Controller
    {
        private readonly IMaintenanceService maintenanceService;

        public ErrorController(IMaintenanceService maintenanceService)
        {
            this.maintenanceService = maintenanceService;
        }

        [Route("Error/404")]
        [Route("Error/PageNotFound")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("Error/500")]
        [Route("Error/InternalError")]
        public IActionResult InternalError()
        {
            return View();
        }

        public async Task<IActionResult> UnderMaintainance()
        {
            var downTime = await maintenanceService.GetUnderMaintenanceTimeAsync();
            ViewBag.UmtFrom = downTime.From;
            ViewBag.UmtTo = downTime.To;

            return View();
        }
    }
}
