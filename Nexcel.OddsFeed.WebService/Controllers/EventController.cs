// --------------------------------------------------------------------------
// <copyright file="EventController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Nexcel.OddsFeed.WebService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;    
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;    
    using System.Web;
    using System.Web.Mvc;
    using System.Xml;
    using MvcContrib.ActionResults;
    using Nexcel.OddsFeed.Library;
    using Nexcel.OddsFeed.Library.Attributes;
    using Nexcel.OddsFeed.WebService.Functions;

    /// <summary>
    /// Class EventController.
    /// </summary>    
    public class EventController : Controller
    {     
        // GET: /Schema/
        [Compress]
        public XmlResult Index(string username)
        {
            var domain = Helper.DomainName(Request.ServerVariables);
            OddsFeedFilter filter = new OddsFeedFilter();
            var data = GetEventFeed(username, HttpContext.Request["t"], domain, filter);
            return new XmlResult(data);
        }

        /// <summary>
        /// Gets the event feed.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="isSample">If set to <c>true</c> [is sample].</param>
        /// <param name="outrights">If set to <c>true</c> [outrights].</param>
        /// <returns>Xml Element.</returns>
        private static XmlElement GetEventFeed(string username, string timestamp, string domain, OddsFeedFilter filter, bool isSample = false, bool outrights = true)
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

            if (err.Id == 0 || isSample)
            {
                var of = new Nexcel.OddsFeed.Library.OddsFeed(partner.OddsProfile, partner.SystemName, domainName, ip, timestamp);
                return of.BuildXmlEvent(partner.Timestamp, filter, isSample, outrights);
            }

            return err.ToXmlElement();
        }
    }
}
