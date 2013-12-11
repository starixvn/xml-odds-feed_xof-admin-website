// --------------------------------------------------------------------------
// <copyright file="CurrentContext.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Web.Context
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Net;
    using System.Web;
    using Fanex.Common.Extensions;

    /// <summary>
    /// Class CurrentContext.
    /// </summary>
    public class CurrentContext : ICurrentContext
    {
        private const string _CONFIGNAPHUBSITEID = "SiteId";       

        public string IP
        {
            get
            {
                string clientIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(clientIp))
                {
                    string[] forwardedIps = clientIp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    clientIp = forwardedIps[forwardedIps.Length - 1];
                }
                else
                {
                    clientIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (clientIp == "::1")
                {
                    clientIp = "127.0.0.1";
                }

                return clientIp;
            }
        }

        ////public string IP
        ////{
        ////    get
        ////    {
        ////        string ip = string.Empty;

        ////        foreach (IPAddress itemIPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
        ////        {
        ////            if (itemIPA.AddressFamily.ToString().Equals("InterNetwork"))
        ////            {
        ////                ip = itemIPA.ToString();
        ////                break;
        ////            }
        ////        }

        ////        if (!ip.IsNullOrTrimEmpty())
        ////        {
        ////            return ip;
        ////        }

        ////        foreach (IPAddress itemIPA in Dns.GetHostAddresses(Dns.GetHostName()))
        ////        {
        ////            if (itemIPA.AddressFamily.ToString().Equals("InterNetwork"))
        ////            {
        ////                ip = itemIPA.ToString();
        ////                break;
        ////            }
        ////        }

        ////        return ip.Trim();
        ////    }
        ////}

        /// <summary>
        /// Gets the i p4 address.
        /// </summary>
        /// <value>The i p4 address.</value>
        public string IP4Address
        {
            get
            {
                string ip = string.Empty;

                foreach (IPAddress ipa in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
                {
                    if (ipa.AddressFamily.ToString().Equals("InterNetwork"))
                    {
                        ip = ipa.ToString();
                        break;
                    }
                }

                if (!ip.Equals(string.Empty))
                {
                    return ip;
                }

                foreach (IPAddress ipa in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (ipa.AddressFamily.ToString().Equals("InterNetwork"))
                    {
                        ip = ipa.ToString();
                        break;
                    }
                }

                return ip.Trim();
            }
        }

        /// <summary>
        /// Gets the i p6 address.
        /// </summary>
        /// <value>The i p6 address.</value>
        public string IP6Address
        {
            get
            {
                // try to get IP from proxy (if any)
                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipsRange = ip.Split(',');
                    string trueIP = ipsRange[0].Trim();
                }
                else
                {
                    // in case client request is not from any proxy
                    ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                return ip;
            }
        }

        public string DomainName
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            }
        }

        /// <summary>
        /// Gets the full name of the domain.
        /// </summary>
        /// <value>The full name of the domain.</value>
        public string FullDomainName
        {
            get
            {
                NameValueCollection variables = HttpContext.Current.Request.ServerVariables;

                string domain = variables["SERVER_NAME"];
                string port = variables["SERVER_PORT"];
                string protocol = HttpContext.Current.Request.IsSecureConnection ? "https://" : "http://";

                var fulldomain = protocol + domain;                

                if (!ConfigurationManager.AppSettings["Environment"].ToString().Equals("Production", StringComparison.OrdinalIgnoreCase)
                    && !ConfigurationManager.AppSettings["Environment"].ToString().Equals("UAT", StringComparison.OrdinalIgnoreCase))
                {
                    fulldomain = fulldomain + ":" + port;
                }

                fulldomain = fulldomain + (HttpRuntime.AppDomainAppVirtualPath == "/" ? string.Empty : HttpRuntime.AppDomainAppVirtualPath);
                return fulldomain;                
            }
        }

        public string BrowserName
        {
            get { return HttpContext.Current.Request.Browser.Browser; }
        }

        public string Platform
        {
            get { return HttpContext.Current.Request.Browser.Platform; }
        }

        public string CountryCode
        {
            get
            {
                RegionInfo regionInfo = ResolveCountry(HttpContext.Current.Request);

                return regionInfo == null ? null : regionInfo.GeoId.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Resolves the culture.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Object CultureInfo.</returns>
        private CultureInfo ResolveCulture(HttpRequest request)
        {
            string[] languages = request.UserLanguages;

            if (languages == null || languages.Length == 0)
            {
                return null;
            }

            try
            {
                string language = languages[0].ToLowerInvariant().Trim();

                return CultureInfo.CreateSpecificCulture(language);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        private RegionInfo ResolveCountry(HttpRequest request)
        {
            CultureInfo culture = ResolveCulture(request);

            if (culture != null)
            {
                return new RegionInfo(culture.LCID);
            }

            return null;
        }
    }
}