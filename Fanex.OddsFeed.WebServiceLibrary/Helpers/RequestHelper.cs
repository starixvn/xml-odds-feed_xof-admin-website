namespace Fanex.OddsFeed.WebServiceLibrary.Helpers
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web;
    using Common;
    using Common.Configurations;
    using Common.Converters;
    using Common.DTO;

    public static class RequestHelper
    {
        public static string GetClientIpAddress()
        {
            var clientIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(clientIp))
            {
                clientIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                var forwardedIps = clientIp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                clientIp = forwardedIps[forwardedIps.Length - 1];
            }

            if (string.Equals(clientIp, "::1"))
            {
                clientIp = "127.0.0.1";
            }

            return clientIp;
        }

        public static string DomainName(NameValueCollection variables)
        {
            var protocol = (variables["SERVER_PORT_SECURE"] == "1") ? "https://" : "http://";
            var domain = variables["SERVER_NAME"];
            var port = variables["SERVER_PORT"];
            var fullDomainName = protocol + domain;

            if (!AppSettings.Environment.Equals("Production", StringComparison.OrdinalIgnoreCase)
                && !AppSettings.Environment.Equals("UAT", StringComparison.OrdinalIgnoreCase))
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
            var fulldomain = protocol + domain;

            if (!AppSettings.Environment.Equals("Production", StringComparison.OrdinalIgnoreCase)
                && !AppSettings.Environment.Equals("UAT", StringComparison.OrdinalIgnoreCase))
            {
                fulldomain = fulldomain + ":" + port;
            }

            fulldomain = fulldomain + (HttpRuntime.AppDomainAppVirtualPath == "/" ? string.Empty : HttpRuntime.AppDomainAppVirtualPath);
            return fulldomain;
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
            var limitedOptionsNumber = AppSettings.LimitedOptions;

            return userQuery.Sports.Count > limitedOptionsNumber ||
                userQuery.Countries.Count > limitedOptionsNumber ||
                userQuery.BetTypes.Count > limitedOptionsNumber ||
                userQuery.Leagues.Count > limitedOptionsNumber;
        }

        private static string StandardizeCountryCodeList(string countryCodeList)
        {
            var countryCodeTable = DbAccess.GetCountryList().ResultSet.Tables[0];
            var filteredRows = countryCodeTable.Select(string.Format("{0} in ('{1}')", "CountryCode", countryCodeList.Replace(",", "','")));
            return string.Join(",", filteredRows.Select(x => x.ItemArray.GetValue(2)).AsEnumerable());
        }
    }
}
