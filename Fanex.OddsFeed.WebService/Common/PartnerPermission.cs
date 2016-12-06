namespace Fanex.OddsFeed.WebService.Common
{
    using System.Text;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Common.DTO;
    using Fanex.OddsFeed.WebService.Functions;
    using Fanex.OddsFeed.WebServiceLibrary.Helpers;
    using WebServiceLibrary.Criteria;
    using WebServiceLibrary.Repositories;

    public sealed class PartnerPermission : ActionFilterAttribute
    {
        private readonly IPartnerRepository _partnerRepository;

        public PartnerPermission()
        {
            _partnerRepository = new PartnerRepository();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var username = filterContext.ActionParameters["username"].ToString();
            var domainName = Helper.DomainName(request.ServerVariables);
            var ipAddress = RequestHelper.GetClientIpAddress();

            var result = GetUserProfileResult(username, domainName, ipAddress);

            if (result.Error.Id > 0)
            {
                filterContext.Result = new ContentResult
                {
                    Content = result.Error.Message,
                    ContentEncoding = Encoding.UTF8,
                    ContentType = "text/xml"
                }; ;
            }
            else
            {
                filterContext.ActionParameters["userProfile"] = result.Data;
            }
        }

        private AuthenticatedPartner GetUserProfileResult(string username, string domainName, string ip)
        {
            return _partnerRepository.GetAuthenticatedPartner(new CheckPermissionCriteria
            {
                UserName = username,
                DomainName = domainName,
                Ip = ip
            });
        }
    }
}