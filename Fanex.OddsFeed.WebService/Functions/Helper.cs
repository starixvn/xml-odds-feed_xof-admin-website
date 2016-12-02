namespace Fanex.OddsFeed.WebService.Functions
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web;
    using OddsFeed.Common;
    using OddsFeed.Common.Converters;
    using OddsFeed.Common.DTO;

    public static class Helper
    {
        public static string DomainName(NameValueCollection variables)
        {
            var protocol = (variables["SERVER_PORT_SECURE"] == "1") ? "https://" : "http://";
            var domain = variables["SERVER_NAME"]; // Request.ServerVariables["HTTP_HOST"].Split(':')[0];
            var port = variables["SERVER_PORT"];
            var fullDomainName = protocol + domain;

            if (!ConfigurationManager.AppSettings["Environment"].ToString().Equals("Production", StringComparison.OrdinalIgnoreCase)
                && !ConfigurationManager.AppSettings["Environment"].ToString().Equals("UAT", StringComparison.OrdinalIgnoreCase))
            {
                fullDomainName = fullDomainName + ":" + port;
            }

            return fullDomainName;
        }

        public static string FullDomainName(NameValueCollection variables)
        {
            var domain = variables["SERVER_NAME"];
            var port = variables["SERVER_PORT"];
            var protocol = (variables["SERVER_PORT_SECURE"] == "1") ? "https://" : "http://";
            ////string protocol = Request.IsSecureConnection ? "https://" : "http://";
            var fulldomain = protocol + domain;

            if (!ConfigurationManager.AppSettings["Environment"].ToString().Equals("Production", StringComparison.OrdinalIgnoreCase)
                && !ConfigurationManager.AppSettings["Environment"].ToString().Equals("UAT", StringComparison.OrdinalIgnoreCase))
            {
                fulldomain = fulldomain + ":" + port;
            }

            fulldomain = fulldomain + (HttpRuntime.AppDomainAppVirtualPath == "/" ? string.Empty : HttpRuntime.AppDomainAppVirtualPath);
            return fulldomain;
        }

        public static string ToCSV(this DataTable dt, bool isPrintHeader, string separateRow)
        {
            var sb = new StringBuilder();
            if (isPrintHeader)
            {
                var columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
                sb.Append(string.Join(",", columnNames));
                sb.Append("__");
            }
            
            foreach (DataRow row in dt.Rows)
            {
                var param = new string[] 
                                                    { 
                                                        row["matchid"].ToString(), row["kickofftime"].ToString(), 
                                                        row["leaguename"].ToString().Replace(",", "\u002C"), row["home"].ToString().Replace(",", "\u002C"), 
                                                        row["away"].ToString().Replace(",", "\u002C") + separateRow 
                                                    };

                sb.Append(string.Join(",", param));                                                
            }

            return sb.ToString();
        }

        public static string ToCSV(this DataRow[] drs, string separateRow)
        {
            var sb = new StringBuilder();            
            foreach (DataRow row in drs)
            {
                // IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Replace(",", "\u002C"));                
                var param = new string[] 
                                                    { 
                                                        row["matchid"].ToString(), row["kickofftime"].ToString(), 
                                                        row["leaguename"].ToString().Replace(",", "\u002C"), row["home"].ToString().Replace(",", "\u002C"), 
                                                        row["away"].ToString().Replace(",", "\u002C") + separateRow 
                                                    };

                sb.Append(string.Join(",", param));
            }

            return sb.ToString();
        }

        public static string ToCSV_Odds(this DataRow[] drs, string separateRow)
        {
            var sb = new StringBuilder();
            ////string StakeOption = string.Empty;

            var param = new string[] { };
            foreach (DataRow row in drs)
            {
                // StakeOption = Math.Floor(Convert.ToDecimal(row["maxbet"]) / 16) + "," + Math.Floor(Convert.ToDecimal(row["maxbet"]) * 4) + separateRow;
                switch (Convert.ToInt16(row["bettype"]))
                {
                    case 1:
                    case 3:
                    case 7:
                    case 8:
                        param = new string[] 
                                                    {
                                                        row["matchid"].ToString(),
                                                        row["oddsid"].ToString(),
                                                        row["bettype"].ToString(),
                                                        row["hdp1"].ToString(),
                                                        row["hdp2"].ToString(),
                                                        row["odds1a"].ToString(),
                                                        row["odds2a"].ToString(),
                                                        row["minbet"].ToString(),
                                                        row["maxbet"].ToString() + separateRow
                                                    };
                        sb.Append(string.Join(",", param));
                        break;
                    case 10:
                    case 11:
                        param = new string[]
                                                    {
                                                        row["matchid"].ToString(),
                                                        row["oddsid"].ToString(),
                                                        row["bettype"].ToString(),
                                                        row["com1"].ToString(),
                                                        row["comX"].ToString(),
                                                        row["com2"].ToString(),
                                                        row["minbet"].ToString(),
                                                        row["maxbet"].ToString() + separateRow
                                                    };
                        sb.Append(string.Join(",", param));
                        break;
                }                
            }

            return sb.ToString();
        }

        public static string ToCSV_Odds_2(this DataRow[] drs, string separateRow)
        {
            var sb = new StringBuilder();
            ////string StakeOption = string.Empty;            
            var param = new string[] { };

            foreach (DataRow row in drs)
            {
                // StakeOption = Math.Floor(Convert.ToDecimal(row["maxbet"]) / 32) + "," + Math.Floor(Convert.ToDecimal(row["maxbet"]) * 6) + separateRow;
                switch (Convert.ToInt16(row["bettype"]))
                {
                    case 4:
                        param = new string[] 
                                                        { 
                                                            row["matchid"].ToString(), row["oddsid"].ToString(), row["bettype"].ToString(), 
                                                            row["cs00"].ToString(), row["cs11"].ToString(), row["cs22"].ToString(), row["cs33"].ToString(),
                                                            row["cs10"].ToString(), row["cs20"].ToString(), row["cs30"].ToString(), 
                                                            row["cs21"].ToString(), row["cs31"].ToString(), row["cs32"].ToString(),
                                                            row["cs01"].ToString(), row["cs02"].ToString(), row["cs03"].ToString(),
                                                            row["cs12"].ToString(), row["cs13"].ToString(), row["cs23"].ToString(),
                                                            row["minbet"].ToString(), row["maxbet"].ToString() + separateRow 
                                                        };
                        sb.Append(string.Join(",", param));                        
                        break;
                    case 16:
                        param = new string[] 
                                                        { 
                                                            row["matchid"].ToString(), row["oddsid"].ToString(), row["bettype"].ToString(), 
                                                            row["cs11"].ToString(), row["cs10"].ToString(), row["cs12"].ToString(),                                                                             
                                                            row["cs01"].ToString(), row["cs00"].ToString(), row["cs02"].ToString(),
                                                            row["cs21"].ToString(), row["cs20"].ToString(), row["cs22"].ToString(),
                                                            row["minbet"].ToString(), row["maxbet"].ToString() + separateRow 
                                                        };
                        sb.Append(string.Join(",", param));
                        break;
                }
            }

            return sb.ToString();
        }

        public static string FooBetToCSV_Odds(this DataRow[] drs, string separateRow)
        {
            var sb = new StringBuilder();
            ////string StakeOption = string.Empty;

            var param = new string[] { };
            foreach (DataRow row in drs)
            {
                // StakeOption = Math.Floor(Convert.ToDecimal(row["maxbet"]) / 16) + "," + Math.Floor(Convert.ToDecimal(row["maxbet"]) * 4) + separateRow;
                switch (Convert.ToInt16(row["bettype"]))
                {
                    case 1:
                    case 3:
                    case 7:
                    case 8:
                        param = new string[] 
                                                    { 
                                                        row["matchid"].ToString(),
                                                        row["oddsid"].ToString(),
                                                        row["bettype"].ToString(),
                                                        row["maxbet"].ToString(),
                                                        row["hdp1"].ToString(),
                                                        row["hdp2"].ToString(),
                                                        row["odds1a"].ToString(),
                                                        row["odds2a"].ToString() + separateRow
                                                    };
                        sb.Append(string.Join(",", param));
                        break;
                    case 10:
                    case 11:
                        param = new string[]
                                                    { 
                                                        row["matchid"].ToString(),
                                                        row["oddsid"].ToString(),
                                                        row["bettype"].ToString(),
                                                        row["maxbet"].ToString(),
                                                        row["com1"].ToString(),
                                                        row["comX"].ToString(),
                                                        row["com2"].ToString() + separateRow
                                                    };
                        sb.Append(string.Join(",", param));
                        break;
                }
            }

            return sb.ToString();
        }

        public static string FooBetToCSV_Odds_2(this DataRow[] drs, string separateRow)
        {
            var sb = new StringBuilder();
            ////string StakeOption = string.Empty;

            foreach (DataRow row in drs)
            {
                // StakeOption = Math.Floor(Convert.ToDecimal(row["maxbet"]) / 32) + "," + Math.Floor(Convert.ToDecimal(row["maxbet"]) * 6) + separateRow;
                switch (Convert.ToInt16(row["bettype"]))
                {                  
                    case 16:
                        var param = new string[] 
                                                        { 
                                                            row["matchid"].ToString(), row["oddsid"].ToString(), row["bettype"].ToString(),
                                                            row["maxbet"].ToString(), 
                                                            row["cs11"].ToString(), row["cs10"].ToString(), row["cs12"].ToString(),                                                                             
                                                            row["cs01"].ToString(), row["cs00"].ToString(), row["cs02"].ToString(),
                                                            row["cs21"].ToString(), row["cs20"].ToString(), row["cs22"].ToString() + separateRow
                                                        };
                        sb.Append(string.Join(",", param));
                        break;
                }
            }

            return sb.ToString();
        }
        
        public static UserQuery GetFilter(HttpRequestBase request)
        {
            var userQuery = new UserQuery();

            var filterSport = request["Sport"] == null ? string.Empty : request["Sport"].ToLower();
            var filterCountry = request["Country"] == null ? string.Empty : request["Country"].ToLower();
            var filterBettype = request["Bettype"] == null ? string.Empty : request["Bettype"].ToLower();
            var filterLeague = request["League"] == null ? string.Empty : request["League"].ToLower();

            if (!string.IsNullOrEmpty(filterCountry))
            {
                filterCountry = StandardizeCountryCodeList(string.Join(",", ListConverter.ConvertToListString(filterCountry, true))).ToLower();
                if (string.IsNullOrEmpty(filterCountry))
                {
                    filterCountry = "null";
                }
            }

            userQuery.Sports = ListConverter.ConvertToListInt(filterSport);
            userQuery.Countries = ListConverter.ConvertToListString(filterCountry, true);
            userQuery.BetTypes = ListConverter.ConvertToListInt(filterBettype);
            userQuery.Leagues = ListConverter.ConvertToListString(filterLeague, true);

            return userQuery;
        }
        
        public static bool IsOverLimitedOptions(UserQuery userQuery)
        {
            var limitedOptionsNumber = Convert.ToInt32(ConfigurationManager.AppSettings["LimitedOptions"]);

            return userQuery.Sports.Count > limitedOptionsNumber ||
                userQuery.Countries.Count > limitedOptionsNumber ||
                userQuery.BetTypes.Count > limitedOptionsNumber ||
                userQuery.Leagues.Count > limitedOptionsNumber;            
        }

        public static string StandardizeCountryCodeList(string countryCodeList)
        {
            var countryCodeTable = DbAccess.GetCountryList().ResultSet.Tables[0];
            var filteredRows = countryCodeTable.Select(string.Format("{0} in ('{1}')", "CountryCode", countryCodeList.Replace(",", "','")));
            return string.Join(",", filteredRows.Select(x => x.ItemArray.GetValue(2)).AsEnumerable());            
        }
    }
}