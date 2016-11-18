namespace Fanex.OddsFeed.WebService.Controllers
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Text;
    using System.Web.Mvc;
    using System.Xml;
    using log4net;
    using MvcContrib.ActionResults;
    using Fanex.OddsFeed.Library;
    using Fanex.OddsFeed.Library.Attributes;
    using Fanex.OddsFeed.WebService.Functions;
    using Library.Services;
    using Library.Configurations;

    public class OddsFeedController : Controller
    {
        private readonly ILog _logService;
        private readonly IBetTypeService _betTypeService;

        public OddsFeedController()
        {
            _logService = LogManager.GetLogger("LogService");
            _betTypeService = new BetTypeService();
        }

        [Compress]
        public XmlResult Index(string username)
        {
            _logService.Info("1. Starting Index action:" + username + "-----" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));

            // check UM Time
            string startTime = string.Empty;
            string endTime = string.Empty;
            if (Utilities.GetDownTime(out startTime, out endTime))
            {
                return new XmlResult(ErrorCollection.UMDownTime(endTime));
            }

            OddsFeedFilter filter = Helper.GetFilter(Request);
            if (Helper.IsOverLimitedOptions(filter))
            {
                Error errMessage = new Error(10024, "You can only filter maximum 5 options. Please try again.");
                return new XmlResult(errMessage.ToXmlElement());
            }

            OddsProfile oddProfile = null;
            var domain = Helper.DomainName(Request.ServerVariables);
            var data = GetOddsFeed(ref oddProfile, username, HttpContext.Request["t"], domain, filter);

            _logService.Info("4. Finishing Index action:" + username + "------" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
            _logService.Info("-------------------------------");
            return new XmlResult(data);
        }

        /// <summary>
        /// Helps the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Action Result.</returns>
        public ActionResult OldHelp(string username)
        {
            // check UM Time
            string startTime = string.Empty;
            string endTime = string.Empty;
            if (Utilities.GetDownTime(out startTime, out endTime))
            {
                return new XmlResult(ErrorCollection.UMDownTime(endTime));
            }

            ViewBag.UserName = username;
            ViewBag.FullDomainName = Helper.FullDomainName(Request.ServerVariables);
            ViewBag.VirtualPath = GetVirtualPath();

            OddsProfile oddProfile = null;
            var domain = Helper.DomainName(Request.ServerVariables);
            OddsFeedFilter filter = new OddsFeedFilter();
            var sampleOdds = GetOddsFeed(ref oddProfile, username, HttpContext.Request["t"], domain, filter, true, true);

            var provider = new StringBuilder(sampleOdds.OuterXml);
            provider.Replace("><", ">\n<");
            provider.Replace(" />", "></odd>");
            provider.Replace("<outrights>", "\t<outrights>")
                .Replace("<event", "\t\t<event")
                .Replace("<odd id", "\t\t\t<odd id")
                .Replace("</event>", "\t\t</event>")
                .Replace("</outrights>", "\t</outrights>")

                .Replace("<matches", "\t<matches")
                .Replace("<match id", "\t\t<match id")
                .Replace("<bookie", "\t\t\t<bookie")
                .Replace("<odd t", "\t\t\t\t<odd t")
                .Replace("</bookie", "\t\t\t</bookie")
                .Replace("</match>", "\t\t</match>")
                .Replace("</matches>", "\t</matches>");

            ViewBag.Odds = provider.ToString();

            ViewBag.Sports = DbConnect.GetSportByUserName(username).ResultSet.Tables[0].Select();
            ViewBag.Bettypes = DbConnect.GetBetTypeByUserName(username).ResultSet.Tables[0].Select();

            if (oddProfile != null)
            {
                return View("Help", oddProfile);
            }
            else
            {
                return new XmlResult(sampleOdds);
            }
        }

        public ActionResult Help(string username)
        {
            // check UM Time
            string startTime = string.Empty;
            string endTime = string.Empty;
            if (Utilities.GetDownTime(out startTime, out endTime))
            {
                return new XmlResult(ErrorCollection.UMDownTime(endTime));
            }

            ViewBag.UserName = username;
            ViewBag.FullDomainName = Helper.FullDomainName(Request.ServerVariables);
            ViewBag.VirtualPath = GetVirtualPath();

            OddsProfile oddProfile = null;
            var domain = Helper.DomainName(Request.ServerVariables);
            OddsFeedFilter filter = new OddsFeedFilter();
            var sampleOdds = GetOddsFeed(ref oddProfile, username, HttpContext.Request["t"], domain, filter, true, true);

            var provider = new StringBuilder(sampleOdds.OuterXml);
            provider.Replace("><", ">\n<");
            provider.Replace(" />", "></odd>");
            provider.Replace("<outrights>", "\t<outrights>")
                .Replace("<event", "\t\t<event")
                .Replace("<odd id", "\t\t\t<odd id")
                .Replace("</event>", "\t\t</event>")
                .Replace("</outrights>", "\t</outrights>")

                .Replace("<matches", "\t<matches")
                .Replace("<match id", "\t\t<match id")
                .Replace("<bookie", "\t\t\t<bookie")
                .Replace("<odd t", "\t\t\t\t<odd t")
                .Replace("</bookie", "\t\t\t</bookie")
                .Replace("</match>", "\t\t</match>")
                .Replace("</matches>", "\t</matches>");

            ViewBag.Odds = provider.ToString();

            ViewBag.ContactEmail = DbConnect.GetContactEmailByUserName(username);

            GenerateAppendixData(username);

            if (oddProfile != null)
            {
                return View("NewHelp", oddProfile);
            }
            else
            {
                return new XmlResult(sampleOdds);
            }
        }

        private void GenerateAppendixData(string username)
        {
            //// Generate supported sports
            DataTable sportsTable = DbConnect.GetSportByUserName(username).ResultSet.Tables[0];
            DataTable sportsCol1 = new DataTable();
            DataTable sportsCol2 = new DataTable();
            DataTable sportsCol3 = new DataTable();
            GenerateData(sportsTable, sportsCol1, sportsCol2, sportsCol3, "SportID", "SportName");
            ViewBag.SportsCol1 = sportsCol1.Select();
            ViewBag.SportsCol2 = sportsCol2.Select();
            ViewBag.SportsCol3 = sportsCol3.Select();

            //// Generate supported bettypes
            DataTable rawBettypesTable = DbConnect.GetBetTypeByUserName(username).ResultSet.Tables[0];
            var bettypesTable = SortDataTable(rawBettypesTable, "BetTypeID", "ASC");
            if (bettypesTable.Rows.Count == 0)
            {
                //// Show Outright bettype (It's an exception)
                DataRow row = bettypesTable.NewRow();
                row["BetTypeID"] = 10;
                row["BetTypeName"] = "Outright";
                bettypesTable.Rows.Add(row);
            }

            DataTable bettypesCol1 = new DataTable();
            DataTable bettypesCol2 = new DataTable();
            DataTable bettypesCol3 = new DataTable();
            GenerateData(bettypesTable, bettypesCol1, bettypesCol2, bettypesCol3, "BetTypeID", "BetTypeName");
            ViewBag.BetTypesCol1 = bettypesCol1.Select();
            ViewBag.BetTypesCol2 = bettypesCol2.Select();
            ViewBag.BetTypesCol3 = bettypesCol3.Select();

            //// Generate supported countries
            DataTable countriesTable = DbConnect.Odds_CountrySelByUserName(username).ResultSet.Tables[0];
            DataTable countriesCol1 = new DataTable();
            DataTable countriesCol2 = new DataTable();
            DataTable countriesCol3 = new DataTable();
            GenerateData(countriesTable, countriesCol1, countriesCol2, countriesCol3, "CountryID", "CountryName");
            ViewBag.CountriesCol1 = countriesCol1.Select();
            ViewBag.CountriesCol2 = countriesCol2.Select();
            ViewBag.CountriesCol3 = countriesCol3.Select();
            ViewBag.HighlightSportType = AppSettings.HighlightSportType.Split(',');
            ViewBag.HighlightBetType = AppSettings.HighlightBetType.Split(',');
            ViewBag.BetTypeList = _betTypeService.GetBetTypeList();
        }

        public static DataTable SortDataTable(DataTable dt, string colName, string direction)
        {
            DataTable dtOut = null;
            dt.DefaultView.Sort = colName + " " + direction;
            dtOut = dt.DefaultView.ToTable();
            return dtOut;
        }

        private void GenerateData(DataTable dt, DataTable column1Data, DataTable column2Data, DataTable column3Data, string columnID, string columnName)
        {
            int sum = dt.Rows.Count;
            //// Just render one column if rowcount< =5
            if (sum <= 5)
            {
                GenerateColumnData(dt, column1Data, column2Data, column3Data, columnID, columnName, sum, 0, 0);
            }
            else
            {
                int divide = sum / 3;
                int module = sum % 3;
                int sumCol1 = divide;
                int sumCol2 = divide;
                int sumCol3 = divide;

                while (module > 0)
                {
                    sumCol2 += 1;
                    module--;

                    if (module > 0)
                    {
                        sumCol3 += 1;
                        module--;
                    }

                    if (module > 0)
                    {
                        sumCol1 += 1;
                        module--;
                    }
                }

                GenerateColumnData(dt, column1Data, column2Data, column3Data, columnID, columnName, sumCol1, sumCol2, sumCol3);
            }
        }

        private void GenerateColumnData(
            DataTable dt,
            DataTable column1Data,
            DataTable column2Data,
            DataTable column3Data,
            string columnID,
            string columnName,
            int sumCol1,
            int sumCol2,
            int sumCol3)
        {
            ////Col1            
            column1Data.Columns.Add(columnID, typeof(string));
            column1Data.Columns.Add(columnName, typeof(string));
            int i = 0;
            while (i < sumCol1)
            {
                DataRow row = column1Data.NewRow();
                row[columnID] = dt.Rows[i][columnID].ToString();
                row[columnName] = dt.Rows[i][columnName].ToString();
                column1Data.Rows.Add(row);
                i++;
            }

            ////Col2
            column2Data.Columns.Add(columnID, typeof(string));
            column2Data.Columns.Add(columnName, typeof(string));
            while (i < sumCol1 + sumCol2)
            {
                DataRow row = column2Data.NewRow();
                row[columnID] = dt.Rows[i][columnID].ToString();
                row[columnName] = dt.Rows[i][columnName].ToString();
                column2Data.Rows.Add(row);
                i++;
            }

            ////Col3            
            column3Data.Columns.Add(columnID, typeof(string));
            column3Data.Columns.Add(columnName, typeof(string));
            while (i < sumCol1 + sumCol2 + sumCol3)
            {
                DataRow row = column3Data.NewRow();
                row[columnID] = dt.Rows[i][columnID].ToString();
                row[columnName] = dt.Rows[i][columnName].ToString();
                column3Data.Rows.Add(row);
                i++;
            }
        }

        private XmlElement GetOddsFeed(
                                                            ref OddsProfile oddProfile,
                                                            string username,
                                                            string timestamp,
                                                            string domain,
                                                            OddsFeedFilter filter,
                                                            bool isSample = false,
                                                            bool outrights = true)
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

            if (partner.OddsProfile != null && (err.Id == 0 || isSample))
            {
                oddProfile = partner.OddsProfile;
                oddProfile.Domain = domainName;

                _logService.Info("2. Get Odds from Cache of " + username + "---" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                var service = new WebOddsFeedService(partner.OddsProfile, partner.SystemName, domainName, ip, timestamp);
                _logService.Info("3. Build XML Document " + username + "---" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                var xmlDocument = service.BuildXmlDocument(partner.Timestamp, filter, isSample, outrights);

                return xmlDocument;
            }

            return err.ToXmlElement();
        }

        /// <summary>
        /// Gets the virtual path.
        /// </summary>
        /// <returns>System String.</returns>
        private string GetVirtualPath()
        {
            string feedUrlPath = ConfigurationManager.AppSettings["FeedUrlPath"].ToString();
            if (!string.IsNullOrEmpty(feedUrlPath))
            {
                feedUrlPath += "/";
            }

            string virtualPathWebService = ConfigurationManager.AppSettings["VirtualPathWebService"].ToString();
            if (!string.IsNullOrEmpty(virtualPathWebService))
            {
                virtualPathWebService += "/";
            }

            return virtualPathWebService + feedUrlPath;
        }
    }
}
