namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.Web.Mvc;
    using Fanex.OddsFeed.WebService.Functions;
    using OddsFeed.Common.Configurations;
    using WebServiceLibrary.Criteria;
    using WebServiceLibrary.Services;

    public class FeedsController : BaseController
    {
        private readonly IFeedsService _feedsService;

        public FeedsController()
        {
            _feedsService = new FeedsService();
        }

        [HttpGet]
        public ActionResult Index(string username, string feedname, int? fromid)
        {
            var ipAddress = GetClientIpAddress();
            var domainName = Helper.DomainName(Request.ServerVariables);
            var authenticatedPartner = GetAuthenticatedPartner(
                new CheckPermissionCriteria
                {
                    UserName = username,
                    DomainName = domainName,
                    Ip = ipAddress
                });

            if (string.IsNullOrEmpty(authenticatedPartner.Error))
            {
                return Content(authenticatedPartner.Error);
            }

            var feedsData = _feedsService.GetFeedsData(
                FormatType.Xml,
                feedname,
                username,
                domainName,
                ipAddress, 
                fromid);

            return Content(feedsData);              
        }

        [HttpGet]
        public ActionResult GetJson(string username, string feedname, int? fromid)
        {
            var ipAddress = GetClientIpAddress();
            var domainName = Helper.DomainName(Request.ServerVariables);
            var authenticatedPartner = GetAuthenticatedPartner(
                new CheckPermissionCriteria
                {
                    UserName = username,
                    DomainName = domainName,
                    Ip = ipAddress
                });

            if(string.IsNullOrEmpty(authenticatedPartner.Error))
            {
                return Content(authenticatedPartner.Error);
            }
           
            var feedsData = _feedsService.GetFeedsData(
                FormatType.Json,
                feedname,
                username,
                domainName,
                ipAddress, 
                fromid);

            return Content(feedsData);        
        }
    }
}
