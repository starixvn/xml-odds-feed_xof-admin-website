namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.Xml;
    using MvcContrib.ActionResults;
    using Fanex.OddsFeed.WebService.Functions;
    using OddsFeed.Common.DTO;
    using WebServiceLibrary.Services;
    using WebServiceLibrary;

    public class EventController : BaseController
    {     
        public XmlResult Index(string username)
        {
            var domain = Helper.DomainName(Request.ServerVariables);
            var userQuery = new UserQuery();
            var data = GetEventFeed(username, HttpContext.Request["t"], domain, userQuery);
            return new XmlResult(data);
        }

        private XmlElement GetEventFeed(string username, string timestamp, string domain, UserQuery userQuery, bool isSample = false, bool outrights = true)
        {
            // ignore port number if it presents
            var domainName = domain;

            if (string.IsNullOrEmpty(username))
            {
                return ErrorBuilder.GetParameterSufficent();
            }

            var ipAddress = GetClientIpAddress();
            var partner = new Partner(username, domainName, ipAddress, timestamp);

            var err = partner.IsAuthenticated();

            if (err.Id == 0 || isSample)
            {
                var oddsFeedService = new EventFeedService(partner.UserProfile);

                return oddsFeedService.BuildXmlEvent(partner.Timestamp, userQuery, isSample, outrights);
            }

            return err.ToXmlElement();
        }
    }
}
