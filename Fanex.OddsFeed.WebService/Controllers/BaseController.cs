namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.IO;
    using System.Web.Mvc;
    using System.Xml;
    using System.Xml.Serialization;
    using OddsFeed.Common.DTO;
    using OddsFeed.Common.Helpers;
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

        protected string SerializeObject<T>(T obj)
        {
            return SerializerHelper.SerializeObject(obj);
        }
    }
}