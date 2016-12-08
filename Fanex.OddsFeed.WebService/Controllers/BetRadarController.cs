namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.Web.Mvc;
    using WebServiceLibrary;
    using WebServiceLibrary.Criteria;
    using WebServiceLibrary.Helpers;
    using WebServiceLibrary.Services;

    public class BetRadarController : BaseController
    {
        private readonly IBetRadarService _betRadarService;

        public BetRadarController()
        {
            _betRadarService = new BetRadarService();
        }

        public ActionResult Index(string userName)
        {
            if (userName.ToLower() != "betradar")
            {
                return Content(ErrorBuilder.GetUnAuthenticated());
            }

            if (IsDownTime())
            {
                var underMaintenanceTime = GetUndermaintenanceTime();

                return Content(ErrorBuilder.GetUMDownTime(underMaintenanceTime.To));
            }
            
            return Content(GetOddsFeed(userName));
        }

        private string GetOddsFeed(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return ErrorBuilder.GetParameterSufficent();
            }

            var authenticatedPartner = GetAuthenticatedPartner(new CheckPermissionCriteria
            {
                UserName = username,
                DomainName = RequestHelper.DomainName(Request.ServerVariables),
                Ip = GetClientIpAddress()
            });
            
            if (string.IsNullOrEmpty(authenticatedPartner.Error))
            {
                return _betRadarService.GetXmlDocument();
            }

            return authenticatedPartner.Error;
        }
    }
}
