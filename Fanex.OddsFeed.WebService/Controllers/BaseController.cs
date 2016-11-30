namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.Web.Mvc;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;

    public class BaseController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;

        public BaseController()
        {
            _maintenanceService = new MaintenanceService();
        }

        protected bool IsDownTime()
        {
            return _maintenanceService.IsUnderMaintenance();
        }
    }
}