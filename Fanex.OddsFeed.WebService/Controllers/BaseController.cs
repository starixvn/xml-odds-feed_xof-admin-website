namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.IO.Compression;
    using System.Text;
    using System.Web.Mvc;
    using OddsFeed.Common.DTO;
    using ONEbook.UM.Models;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;
    using WebServiceLibrary;
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

        protected string GetDowntimeContent()
        {
            return ErrorBuilder.GetUMDownTime(GetUndermaintenanceTime().To);
        }

        protected AuthenticatedPartner GetAuthenticatedPartner(string userName, string domainName, string ip)
        {
            return _partnerRepository.GetAuthenticatedPartner(new CheckPermissionCriteria
            {
                UserName = userName,
                DomainName = domainName,
                Ip = ip
            });
        }

        protected static string GetClientIpAddress()
        {
            return RequestHelper.GetClientIpAddress();
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var encodingsAccepted = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(encodingsAccepted)) return;

            encodingsAccepted = encodingsAccepted.ToLowerInvariant();
            var response = filterContext.HttpContext.Response;

            if (encodingsAccepted.Contains("deflate"))
            {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
            else if (encodingsAccepted.Contains("gzip"))
            {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
        }
        protected ActionResult XmlContent(string data)
        {
            return Content(data, "application/xml", Encoding.UTF8);
        }
    }
}