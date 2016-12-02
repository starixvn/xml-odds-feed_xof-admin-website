namespace Fanex.OddsFeed.WebService.Controllers
{
    using System;
    using System.Xml.Linq;
    using MvcContrib.ActionResults;
    using Fanex.OddsFeed.WebService.Functions;
    using OddsFeed.Common.DTO;

    public class FeedsController : BaseController
    {
        public XmlResult Index(string username, string feedname, int? fromid)
        {
            var typeName = "Fanex.OddsFeed.Library.Model." + feedname +
                ", Fanex.OddsFeed.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

            var feedType = Type.GetType(typeName, false, true);

            if (feedType == null)
            {
                return new XmlResult(new Error(-1, "This feed is not available.").ToXElement());
            }            
                    
            var domain = Helper.DomainName(Request.ServerVariables);                                     
            var data = GetXmlFeeds(feedType, username, HttpContext.Request["t"], domain, fromid);
            return new XmlResult(data);                     
        }

        public string GetJson(string username, string feedname, int? fromid)
        {
            var typeName = "Fanex.OddsFeed.Library.Model." + feedname +
                ", Fanex.OddsFeed.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

            var feedType = Type.GetType(typeName, false, true);

            if (feedType == null)
            {
                return new Error { Id = -1, Message = "This feed is not available." };
            }

            var domain = Helper.DomainName(Request.ServerVariables);                                     
            var data = GetJsonFeeds(feedType, username, HttpContext.Request["t"], domain, fromid);
            return data;            
        }

        private string GetJsonFeeds(Type feedType, string username, string timestamp, string domain, int? fromid)
        {
            var ipAddress = GetClientIpAddress();

            var err = IsAuthenticated(username, timestamp, domain, ipAddress);

            if (err.HasValue && err.Value.Id == 0)
            {
                var genericType = typeof(Feeds<>);
                var a = genericType.MakeGenericType(feedType);
                dynamic obj = Activator.CreateInstance(a);

                return obj.BuildJson(domain, ipAddress, username, fromid);
            }

            return err.Value.ToString();
        }

        private XContainer GetXmlFeeds(Type feedType, string username, string timestamp, string domain, int? fromid)
        {
            var ipAddress = GetClientIpAddress();

            var err = IsAuthenticated(username, timestamp, domain, ipAddress);

            if (err.HasValue && err.Value.Id == 0)
            {
                var genericType = typeof(Feeds<>);
                var a = genericType.MakeGenericType(feedType);
                dynamic obj = Activator.CreateInstance(a);

                return obj.BuildXmlDocument(domain, ipAddress, username, fromid);
            }
            XmlResult()
            return err.Value.ToXElement();
        }

        private static Error IsAuthenticated(string username, string timestamp, string domain, string ip)
        {
            var domainName = domain;

            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            var partner = new Partner(username, domainName, ip, timestamp);

            return partner.IsAuthenticated();
        }
    }
}
