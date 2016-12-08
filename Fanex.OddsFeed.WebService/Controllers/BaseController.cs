namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.Text;
    using System.Web.Mvc;
    using OddsFeed.Common.DTO;
    using ONEbook.UM.Models;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;
    using WebServiceLibrary.Criteria;
    using WebServiceLibrary.Helpers;
    using WebServiceLibrary.Repositories;

    public class BaseController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly IPartnerRepository _partnerRepository;

        public BaseController()
        {
            _maintenanceService = new MaintenanceService();
            _partnerRepository = new PartnerRepository();
        }

        protected bool IsDownTime()
        {
            return _maintenanceService.IsUnderMaintenance();
        }

        protected UnderMaintenanceTime GetUndermaintenanceTime()
        {
            return _maintenanceService.GetUnderMaintenanceTime();
        }

        protected AuthenticatedPartner GetAuthenticatedPartner(CheckPermissionCriteria criteria)
        {
            return _partnerRepository.GetAuthenticatedPartner(criteria);
        }

        protected string GetClientIpAddress()
        {
            return RequestHelper.GetClientIpAddress();
        }

        protected void ChangeToReponseXml()
        {
            Response.ContentType = "application/xml";
            Response.ContentEncoding = Encoding.UTF8;
        }
    }
}