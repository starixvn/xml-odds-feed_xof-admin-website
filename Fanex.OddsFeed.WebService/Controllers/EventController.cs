namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.Web.Mvc;
    using System.Xml;
    using MvcContrib.ActionResults;
    using Fanex.OddsFeed.Library;
    using Fanex.OddsFeed.Library.Attributes;
    using Fanex.OddsFeed.WebService.Functions;
    using Library.Services;

    public class EventController : Controller
    {     
        [Compress]
        public XmlResult Index(string username)
        {
            var domain = Helper.DomainName(Request.ServerVariables);
            var filter = new OddsFeedFilter();
            var data = GetEventFeed(username, HttpContext.Request["t"], domain, filter);
            return new XmlResult(data);
        }

        private static XmlElement GetEventFeed(string username, string timestamp, string domain, OddsFeedFilter filter, bool isSample = false, bool outrights = true)
        {
            // ignore port number if it presents
            var domainName = domain;

            if (string.IsNullOrEmpty(username))
            {
                return ErrorCollection.ParameterSufficent();
            }

            var ip = Utilities.GetIpAddressOfClient();
            var partner = new Partner(username, domainName, ip, timestamp);

            var err = partner.IsAuthenticated();

            if (err.Id == 0 || isSample)
            {
                var oddsFeedService = new EventOddsFeedService(partner.OddsProfile);

                return oddsFeedService.BuildXmlEvent(partner.Timestamp, filter, isSample, outrights);
            }

            return err.ToXmlElement();
        }
    }
}
