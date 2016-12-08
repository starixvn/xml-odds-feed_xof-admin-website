namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.Web.Mvc;
    using WebServiceLibrary;
    using WebServiceLibrary.Criteria;
    using WebServiceLibrary.Helpers;
    using WebServiceLibrary.Services;

    public class EventController : BaseController
    {
        public ActionResult Index(string username)
        {
            var domain = RequestHelper.DomainName(Request.ServerVariables);
            var data = GetEventFeed(username, HttpContext.Request["t"], domain);

            return Content(data);
        }

        private string GetEventFeed(string username, string timestamp, string domain, bool isSample = false, bool outrights = true)
        {
            var domainName = domain;

            if (string.IsNullOrEmpty(username))
            {
                return ErrorBuilder.GetParameterSufficent();
            }

            var authenticatedPartner = GetAuthenticatedPartner(new CheckPermissionCriteria
            {
                UserName = username,
                DomainName = domain,
                Ip = GetClientIpAddress()
            });

            if (string.IsNullOrEmpty(authenticatedPartner.Error) || isSample)
            {
                var oddsFeedService = new EventFeedService(authenticatedPartner.UserProfile);

                return oddsFeedService.BuildXmlEvent(isSample, outrights);
            }

            return authenticatedPartner.Error;
        }
    }
}