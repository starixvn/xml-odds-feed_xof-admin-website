// --------------------------------------------------------------------------
// <copyright file="FeedsController.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>
namespace Fanex.OddsFeed.WebService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;    
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;        
    using System.Xml.Linq;    
    using MvcContrib.ActionResults;    
    using Fanex.OddsFeed.Library;
    using Fanex.OddsFeed.Library.Attributes;
    using Fanex.OddsFeed.Library.Model;
    using Fanex.OddsFeed.WebService.Functions;

    /// <summary>
    /// Class FeedsController.
    /// </summary>    
    public class FeedsController : Controller
    {
        [Compress]
        public XmlResult Index(string username, string feedname, int? fromid)
        {            
            var a = typeof(Country).AssemblyQualifiedName;
            var typeName = "Fanex.OddsFeed.Library.Model." + feedname +
                ", Fanex.OddsFeed.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

            var feedType = Type.GetType(typeName, false, true);

            if (feedType == null)
            {
                return new XmlResult(new Error(-1, "This feed is not available.").ToXElement());
            }            
                    
            var domain = Helper.DomainName(Request.ServerVariables);                                     
            var data = GetXmlFeeds(feedType, username, HttpContext.Request["t"], domain, fromid);
            return new XmlResult(data);                     
        }

        public string GetJson(string username, string feedname, int? fromid)
        {
            var a = typeof(Country).AssemblyQualifiedName;
            var typeName = "Fanex.OddsFeed.Library.Model." + feedname +
                ", Fanex.OddsFeed.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

            var feedType = Type.GetType(typeName, false, true);

            if (feedType == null)
            {
                return new Error(-1, "This feed is not available.").ToString();
            }

            var domain = Helper.DomainName(Request.ServerVariables);                                     
            var data = GetJsonFeeds(feedType, username, HttpContext.Request["t"], domain, fromid);
            return data;            
        }

        private static string GetJsonFeeds(Type feedType, string username, string timestamp, string domain, int? fromid)
        {
            var ip = Utilities.GetIpAddressOfClient();

            var err = IsAuthenticated(username, timestamp, domain, ip);

            if (err.HasValue && err.Value.Id == 0)
            {
                var genericType = typeof(Feeds<>);
                var a = genericType.MakeGenericType(feedType);
                dynamic obj = Activator.CreateInstance(a);

                return obj.BuildJson(domain, ip, username, fromid);
            }

            return err.Value.ToString();
        }

        private static XContainer GetXmlFeeds(Type feedType, string username, string timestamp, string domain, int? fromid)
        {
            var ip = Utilities.GetIpAddressOfClient();

            var err = IsAuthenticated(username, timestamp, domain, ip);

            if (err.HasValue && err.Value.Id == 0)
            {
                var genericType = typeof(Feeds<>);
                var a = genericType.MakeGenericType(feedType);
                dynamic obj = Activator.CreateInstance(a);

                return obj.BuildXmlDocument(domain, ip, username, fromid);
            }

            return err.Value.ToXElement();
        }

        private static Error? IsAuthenticated(string username, string timestamp, string domain, string ip)
        {
            // ignore port number if it presents
            var domainName = domain;

            if (string.IsNullOrEmpty(username))
            {
                return null; // ErrorCollection.ParameterSufficent();
            }

            var partner = new Partner(username, domainName, ip, timestamp);

            return partner.IsAuthenticated();
        }
    }
}
