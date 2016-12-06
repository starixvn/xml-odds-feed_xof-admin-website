namespace Fanex.OddsFeed.WebService.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Xml.Linq;
    using Fanex.OddsFeed.WebService.Functions;
    using OddsFeed.Common.DTO;
    using WebServiceLibrary.Criteria;

    public class FeedsController : BaseController
    {
        public ActionResult Index(string username, string feedname, int? fromid)
        {
            var typeName = "Fanex.OddsFeed.Library.Model." + feedname +
                ", Fanex.OddsFeed.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

            var feedType = Type.GetType(typeName, false, true);

            if (feedType == null)
            {
                return Content(SerializeObject(
                    new Error
                    {
                        Id = -1,
                        Message = "This feed is not available."
                    }));
            }            
                    
            var domain = Helper.DomainName(Request.ServerVariables);                                     
            var data = GetXmlFeeds(feedType, username, HttpContext.Request["t"], domain, fromid);

            return Content(data);                     
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
            return err.Value.ToXElement();
        }

        private QueryResult<UserProfile> IsAuthenticated(string username, string timestamp, string domain, string ip)
        {
            var domainName = domain;

            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            return GetAuthenticatedUserProfile(new CheckPermissionCriteria { UserName = username, DomainName = domain, Ip = ip });
        }
    }
}
