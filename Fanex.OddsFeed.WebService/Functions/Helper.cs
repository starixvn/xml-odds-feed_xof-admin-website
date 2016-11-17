namespace Fanex.OddsFeed.WebService.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Fanex.OddsFeed.Library;

    /// <summary>
    /// Class Helper.
    /// </summary>    
    public static class Helper
    {
        #region Domain

        /// <summary>
        /// Get the name of the domain.
        /// </summary>
        /// <param name="variables">The variables.</param>
        /// <returns>System String.</returns>
        public static string DomainName(NameValueCollection variables)
        {
            string protocol = (variables["SERVER_PORT_SECURE"] == "1") ? "https://" : "http://";
            string domain = variables["SERVER_NAME"]; // Request.ServerVariables["HTTP_HOST"].Split(':')[0];
            string port = variables["SERVER_PORT"];
            string fullDomainName = protocol + domain;

            if (!ConfigurationManager.AppSettings["Environment"].ToString().Equals("Production", StringComparison.OrdinalIgnoreCase)
                && !ConfigurationManager.AppSettings["Environment"].ToString().Equals("UAT", StringComparison.OrdinalIgnoreCase))
            {
                fullDomainName = fullDomainName + ":" + port;
            }

            return fullDomainName;
        }

        /// <summary>
        /// Get the full name of the domain.
        /// </summary>
        /// <param name="variables">The variables.</param>
        /// <returns>System String.</returns>
        public static string FullDomainName(NameValueCollection variables)
        {
            string domain = variables["SERVER_NAME"];
            string port = variables["SERVER_PORT"];
            string protocol = (variables["SERVER_PORT_SECURE"] == "1") ? "https://" : "http://";
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

        #endregion       

        #region ToCSV

        /// <summary>
        /// To the CSV.
        /// </summary>
        /// <param name="dt">The data table.</param>
        /// <param name="isPrintHeader">If set to <c>true</c> [is print header].</param>
        /// <param name="separateRow">The separate row.</param>
        /// <returns>System String.</returns>
        public static string ToCSV(this DataTable dt, bool isPrintHeader, string separateRow)
        {
            StringBuilder sb = new StringBuilder();
            if (isPrintHeader)
            {
                IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
                sb.Append(string.Join(",", columnNames));
                sb.Append("__");
            }
            
            foreach (DataRow row in dt.Rows)
            {
                string[] param = new string[] 
                                                    { 
                                                        row["matchid"].ToString(), row["kickofftime"].ToString(), 
                                                        row["leaguename"].ToString().Replace(",", "\u002C"), row["home"].ToString().Replace(",", "\u002C"), 
                                                        row["away"].ToString().Replace(",", "\u002C") + separateRow 
                                                    };

                sb.Append(string.Join(",", param));                                                
            }

            return sb.ToString();
        }

        /// <summary>
        /// To the CSV.
        /// </summary>
        /// <param name="drs">The data row.</param>
        /// <param name="separateRow">The separate row.</param>
        /// <returns>System String.</returns>
        public static string ToCSV(this DataRow[] drs, string separateRow)
        {
            StringBuilder sb = new StringBuilder();            
            foreach (DataRow row in drs)
            {
                // IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString().Replace(",", "\u002C"));                
                string[] param = new string[] 
                                                    { 
                                                        row["matchid"].ToString(), row["kickofftime"].ToString(), 
                                                        row["leaguename"].ToString().Replace(",", "\u002C"), row["home"].ToString().Replace(",", "\u002C"), 
                                                        row["away"].ToString().Replace(",", "\u002C") + separateRow 
                                                    };

                sb.Append(string.Join(",", param));
            }

            return sb.ToString();
        }

        #endregion

        #region ToCSV_Odds

        /// <summary>
        /// To the cs v_ odds.
        /// </summary>
        /// <param name="drs">The data row.</param>
        /// <param name="separateRow">The separate row.</param>
        /// <returns>System String.</returns>
        public static string ToCSV_Odds(this DataRow[] drs, string separateRow)
        {
            StringBuilder sb = new StringBuilder();            
            ////string StakeOption = string.Empty;

            string[] param = new string[] { };
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

        /// <summary>
        /// To the cs v_ odds_2.
        /// </summary>
        /// <param name="drs">The data row.</param>
        /// <param name="separateRow">The separate row.</param>
        /// <returns>System String.</returns>
        public static string ToCSV_Odds_2(this DataRow[] drs, string separateRow)
        {
            StringBuilder sb = new StringBuilder();
            ////string StakeOption = string.Empty;            
            string[] param = new string[] { };

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

        /// <summary>
        /// Generate foo bet to cs v_ odds.
        /// </summary>
        /// <param name="drs">The data row.</param>
        /// <param name="separateRow">The separate row.</param>
        /// <returns>System String.</returns>
        public static string FooBetToCSV_Odds(this DataRow[] drs, string separateRow)
        {
            StringBuilder sb = new StringBuilder();
            ////string StakeOption = string.Empty;

            string[] param = new string[] { };
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

        /// <summary>
        /// Generate foo bet to cs v_ odds_2.
        /// </summary>
        /// <param name="drs">The data row.</param>
        /// <param name="separateRow">The separate row.</param>
        /// <returns>System String.</returns>
        public static string FooBetToCSV_Odds_2(this DataRow[] drs, string separateRow)
        {
            StringBuilder sb = new StringBuilder();
            ////string StakeOption = string.Empty;

            foreach (DataRow row in drs)
            {
                // StakeOption = Math.Floor(Convert.ToDecimal(row["maxbet"]) / 32) + "," + Math.Floor(Convert.ToDecimal(row["maxbet"]) * 6) + separateRow;
                switch (Convert.ToInt16(row["bettype"]))
                {                  
                    case 16:
                        string[] param = new string[] 
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

        #endregion        

        #region Filter

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Object OddsFeedFilter.</returns>
        public static OddsFeedFilter GetFilter(HttpRequestBase request)
        {
            OddsFeedFilter oddsFeedFilter = new OddsFeedFilter();

            string filterSport = request["Sport"] == null ? string.Empty : request["Sport"].ToLower();
            string filterCountry = request["Country"] == null ? string.Empty : request["Country"].ToLower();
            string filterBettype = request["Bettype"] == null ? string.Empty : request["Bettype"].ToLower();
            string filterLeague = request["League"] == null ? string.Empty : request["League"].ToLower();

            if (!string.IsNullOrEmpty(filterCountry))
            {
                filterCountry = StandardizeCountryCodeList(string.Join(",", Utilities.GetStringIList(filterCountry, true))).ToLower();
                if (string.IsNullOrEmpty(filterCountry))
                {
                    filterCountry = "null";
                }
            }

            oddsFeedFilter.Sports = Utilities.GetIntList(filterSport);
            oddsFeedFilter.Countries = Utilities.GetStringIList(filterCountry, true);
            oddsFeedFilter.BetTypes = Utilities.GetIntList(filterBettype);
            oddsFeedFilter.Leagues = Utilities.GetStringIList(filterLeague, true);

            return oddsFeedFilter;
        }

        /// <summary>
        /// Check over limited options.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>Return True if over else return false.</returns>
        public static bool IsOverLimitedOptions(OddsFeedFilter filter)
        {
            int limitedOptionsNumber = Convert.ToInt32(ConfigurationManager.AppSettings["LimitedOptions"]);

            return filter.Sports.Count > limitedOptionsNumber ||
                filter.Countries.Count > limitedOptionsNumber ||
                filter.BetTypes.Count > limitedOptionsNumber ||
                filter.Leagues.Count > limitedOptionsNumber;            
        }

        /// <summary>
        /// Standardize list of country codes.
        /// </summary>
        /// <param name="countryCodeList">The country code list.</param>
        /// <returns>The standardize country code list.</returns>
        public static string StandardizeCountryCodeList(string countryCodeList)
        {
            DataTable countryCodeTable = DbConnect.GetCountryList().ResultSet.Tables[0];
            DataRow[] filteredRows = countryCodeTable.Select(string.Format("{0} in ('{1}')", "CountryCode", countryCodeList.Replace(",", "','")));
            return string.Join(",", filteredRows.Select(x => x.ItemArray.GetValue(2)).AsEnumerable());            
        }
     
        #endregion
    }
}