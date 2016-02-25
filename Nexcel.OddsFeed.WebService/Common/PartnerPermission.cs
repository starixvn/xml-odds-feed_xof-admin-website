using Nexcel.OddsFeed.Library;
using Nexcel.OddsFeed.WebService.Functions;
using System.Text;
using System.Web.Mvc;

namespace Nexcel.OddsFeed.WebService.Common
{
    public class PartnerPermission : ActionFilterAttribute, IActionFilter
    {
        private const string XmlPrefix = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)


        {
            var request = filterContext.HttpContext.Request;
            var username = filterContext.ActionParameters["username"].ToString();
            var domainName = Helper.DomainName(request.ServerVariables);
            var ip = Utilities.GetIpAddressOfClient();

            string errorMessage;
            var oddsProfile = GetOddsProfile(username, domainName, ip, out errorMessage);
            if (oddsProfile == null)
            {
                var result = new ContentResult
                {
                    Content = errorMessage,
                    ContentEncoding = Encoding.UTF8,
                    ContentType = "text/xml"
                };

                filterContext.Result = result;
            }
            else
            {
                filterContext.ActionParameters["oddsProfile"] = oddsProfile;
            }
        }

        private OddsProfile GetOddsProfile(string username, string domainName, string ip, out string errorMessage)
        {
            var partner = new Partner(username, domainName, ip, null);
            var error = partner.IsAuthenticated();

            if (error.Id == 0)
            {
                var oddsProfile = partner.OddsProfile;
                oddsProfile.Domain = domainName;
                errorMessage = string.Empty;
                return oddsProfile;
            }
            else
            {
                errorMessage = XmlPrefix + error.ToXmlElement().OuterXml;
                return null;
            }

        }
    }
}