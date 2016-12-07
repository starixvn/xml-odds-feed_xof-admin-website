namespace Fanex.OddsFeed.WebService.Controllers
{
    using System.Data;
    using System.Web.Mvc;
    using System.Xml;
    using Fanex.OddsFeed.WebService.Functions;
    using OddsFeed.Common.Formatter;
    using WebServiceLibrary;
    using WebServiceLibrary.Criteria;
    using WebServiceLibrary.Services;

    public class BetRadarController : BaseController
    {
        private readonly IBetRadarService _betRadarService;

        public BetRadarController()
        {
            _betRadarService = new BetRadarService();
        }

        public ActionResult Index(string userName)
        {
            if (userName.ToLower() != "betradar")
            {
                return Content(ErrorBuilder.GetUnAuthenticated());
            }

            if (IsDownTime())
            {
                var underMaintenanceTime = GetUndermaintenanceTime();

                return Content(ErrorBuilder.GetUMDownTime(underMaintenanceTime.To));
            }
            
            return Content(GetOddsFeed(userName));
        }

        private string GetOddsFeed(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return ErrorBuilder.GetParameterSufficent();
            }

            var authenticatedPartner = GetAuthenticatedPartner(new CheckPermissionCriteria
            {
                UserName = username,
                DomainName = Helper.DomainName(Request.ServerVariables),
                Ip = GetClientIpAddress()
            });
            
            if (string.IsNullOrEmpty(authenticatedPartner.Error))
            {
                return _betRadarService.GetXmlDocument();
            }

            return authenticatedPartner.Error;
        }

        private XmlElement CreateNode(XmlDocument xd, string nodeName, string value)
        {
            var node = xd.CreateElement(nodeName);
            node.InnerText = value;
            return node;
        }

        private XmlElement OddsDataThreeway(XmlDocument xd, DataRow row)
        {
            var node = xd.CreateElement("OddsData");
            node.SetAttribute("type", "3w");
            node.AppendChild(CreateNode(xd, "AC", "3"));
            node.AppendChild(CreateNode(xd, "HomeOdds", NumberFormatter.FormatOdds(row["com1_5"])));
            node.AppendChild(CreateNode(xd, "DrawOdds", NumberFormatter.FormatOdds(row["comx_5"])));
            node.AppendChild(CreateNode(xd, "AwayOdds", NumberFormatter.FormatOdds(row["com2_5"])));
            return node;
        }

        private XmlElement OddsDataHandicap(XmlDocument xd, DataRow row)
        {
            var node = xd.CreateElement("OddsData");
            node.SetAttribute("type", "handicap");
            node.AppendChild(CreateNode(xd, "AC", "1"));
            node.AppendChild(CreateNode(xd, "Handicap", row["hdp1_1"].ToString() + ":" + row["hdp2_1"].ToString()));
            node.AppendChild(CreateNode(xd, "HomeOdds", NumberFormatter.FormatOdds(row["odds1hka_1"])));
            node.AppendChild(CreateNode(xd, "AwayOdds", NumberFormatter.FormatOdds(row["odds2hka_1"])));
            return node;
        }

        private XmlElement OddsDataOverUnder(XmlDocument xd, DataRow row)
        {
            var node = xd.CreateElement("OddsData");
            node.SetAttribute("type", "total");
            node.AppendChild(CreateNode(xd, "AC", "1"));
            node.AppendChild(CreateNode(xd, "Total", NumberFormatter.FormatTotal(row["Hdp1_3"])));
            node.AppendChild(CreateNode(xd, "UnderOdds", NumberFormatter.FormatOdds(row["odds1hka_3"])));
            node.AppendChild(CreateNode(xd, "OverOdds", NumberFormatter.FormatOdds(row["odds2hka_3"])));
            return node;
        }
    }
}
