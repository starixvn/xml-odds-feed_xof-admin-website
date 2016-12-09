namespace Fanex.OddsFeed.WebService.Controllers
{
    using WebServiceLibrary.Criteria;
    using WebServiceLibrary.Helpers;
    using WebServiceLibrary.Services;

    public class BetDaqController : BaseController
    {
        private readonly IBetDaqService _betdaqService;

        public BetDaqController()
        {
            _betdaqService = new BetDaqService();
        }

        public string GetSchedule(string userName)
        {
            if (userName.ToLower() != "betdaq")
            {
                return "You are not authorized to take this action.";
            }
            
            if (IsDownTime())
            {
                return string.Empty;
            }
       
            return GetSchedule(userName, RequestHelper.DomainName(Request.ServerVariables), HttpContext.Request["s"]);
        }
        
        public string GetOdds(string userName)
        {
            if (userName.ToLower() != "betdaq")
            {
                return "You are not authorized to take this action.";
            }

            if (IsDownTime())
            {
                return string.Empty;
            }

            return GetOdds(userName, RequestHelper.DomainName(Request.ServerVariables), HttpContext.Request["s"]);
        }
       
        private string GetSchedule(string username, string domainName, string filter)
        {
            if (string.IsNullOrEmpty(username))
            {
                return string.Empty;
            }
            
            var authenticatedPartner = GetAuthenticatedPartner(username, domainName, GetClientIpAddress());

            if (string.IsNullOrEmpty(authenticatedPartner.Error))
            {
                return _betdaqService.GetSchedule(filter);
            }

            return string.Empty;
        }

        private string GetOdds(string username, string domainName, string filter = null)
        {
            if (string.IsNullOrEmpty(username))
            {
                return string.Empty;
            }

            var authenticatedPartner = GetAuthenticatedPartner(username, domainName, GetClientIpAddress());
            
            if (string.IsNullOrEmpty(authenticatedPartner.Error))
            {
                return _betdaqService.GetOddWithUserIsBetdaq(filter);                  
            }

            return string.Empty;
        }
    }
}
