namespace Fanex.OddsFeed.WebService.Controllers
{
    using System;
    using System.Collections;
    using OddsFeed.Common.Configurations;
    using WebServiceLibrary.Helpers;
    using WebServiceLibrary.Services;

    public class InsertBetdaqController : BaseController
    {
        private readonly IBetDaqService _betDaqService;

        public InsertBetdaqController()
        {
            _betDaqService = new BetDaqService();
        }

        public string Index(string username)
        {
            if (!username.Equals("betdaq", StringComparison.OrdinalIgnoreCase))
            {
                return "You are not authorized to take this action.";
            }

            if (!AppSettings.IsDisableUMTimeFlag && IsDownTime())
            {
                return "Failed - Bad oddsId";
            }

            return InsertBetdaq(username, RequestHelper.DomainName(Request.ServerVariables));
        }

        private string InsertBetdaq(string username, string domainName)
        {
            var ipAddress = GetClientIpAddress();
            var isAuthenticated = IsAuthenticatedPartner(username, domainName, ipAddress);

            if (!isAuthenticated)
            {
                return "Failed - Bad oddsId";
            }

            return GetMessage(_betDaqService.InsertBetDaq(GetSpParams(ipAddress), IsDownTime()));
        }

        private Hashtable GetSpParams(string ipAddress)
        {
            return new Hashtable
            {
                { "oddsid", Request["OddsId"] },
                { "hdp1", Request["Handicap1"] },
                { "hdp2", Request["Handicap2"] },
                { "betteam", Request["BetTeam"] },
                { "odds", Request["Odds"] },
                { "stake", Request["Stake"] },
                { "betdaqid", Request["RequestId"] },
                { "bet_ip", ipAddress }
            };
        }

        private static string GetMessage(int errorCode)
        {
            switch (errorCode)
            {
                case 0:
                    return "OK";                        
                case 1:
                    return "Failed - Bad oddsId";                        
                case 2:
                    return "OK Already";                      
                default:
                    return "Failed - Bad oddsId";                        
            }
        }
    }
}
