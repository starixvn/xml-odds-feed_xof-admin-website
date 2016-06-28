// --------------------------------------------------------------------------
// <copyright file="FeedsController.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
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
    using System.Globalization;    
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;        
    using System.Xml.Linq;    
    using MvcContrib.ActionResults;    
    using Nexcel.OddsFeed.Library;
    using Nexcel.OddsFeed.Library.Attributes;
    using Nexcel.OddsFeed.Library.Model;
    using Nexcel.OddsFeed.WebService.Functions;

    /// <summary>
    /// Class FeedsController.
    /// </summary>    
    public class FeedsController : Controller
    {
        /// <summary>
        /// Indexes the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="feedname">The feed name.</param>
        /// <param name="fromid">The from id.</param>
        /// <returns>Xml Result.</returns>
        [Compress]
        public XmlResult Index(string username, string feedname, int? fromid)
        {            
            var a = typeof(Country).AssemblyQualifiedName;
            var typeName = "Nexcel.OddsFeed.Library.Model." + feedname +
                ", Nexcel.OddsFeed.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

            Type feedType = Type.GetType(typeName, false, true);

            if (feedType == null)
            {
                return new XmlResult(new Error(-1, "This feed is not available.").ToXElement());
            }            

            // try
            // {            
            var domain = Helper.DomainName(Request.ServerVariables);                                     
            var data = GetXmlFeeds(feedType, username, HttpContext.Request["t"], domain, fromid);
            return new XmlResult(data);

            // }
            // catch(Exception ex)
            // {
            //     return new XmlResult(new Error(-1, ex.Message));
            // }                       
        }

        /// <summary>
        /// Gets the JSON.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="feedname">The feed name.</param>
        /// <param name="fromid">The from id.</param>
        /// <returns>System String.</returns>
        public string GetJson(string username, string feedname, int? fromid)
        {
            var a = typeof(Country).AssemblyQualifiedName;
            var typeName = "Nexcel.OddsFeed.Library.Model." + feedname +
                ", Nexcel.OddsFeed.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

            Type feedType = Type.GetType(typeName, false, true);

            if (feedType == null)
            {
                return new Error(-1, "This feed is not available.").ToString();
            }

            var domain = Helper.DomainName(Request.ServerVariables);                                     
            var data = GetJsonFeeds(feedType, username, HttpContext.Request["t"], domain, fromid);
            return data;            
        }

        /// <summary>
        /// Gets the JSON feeds.
        /// </summary>
        /// <param name="feedType">Type of the feed.</param>
        /// <param name="username">The username.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="fromid">The from id.</param>
        /// <returns>System String.</returns>
        private string GetJsonFeeds(Type feedType, string username, string timestamp, string domain, int? fromid)
        {
            var ip = Utilities.GetIpAddressOfClient();

            var err = IsAuthenticated(username, timestamp, domain, ip);

            if (err.HasValue && err.Value.Id == 0)
            {
                ////try
                ////{
                Type genericType = typeof(Feeds<>);
                Type a = genericType.MakeGenericType(feedType);
                dynamic obj = Activator.CreateInstance(a);

                return obj.BuildJson(domain, ip, username, fromid);
                ////}
                ////catch (Exception e)
                ////{
                ////    Helper.LogError(e.Message);
                ////    string errMsg = "Sorry for the inconvenience! We are not able to process your request this time. Please try again later";
                ////    return new Error(4, errMsg).ToXElement();
                ////}
            }

            return err.Value.ToString();
        }

        /// <summary>
        /// Gets the XML feeds.
        /// </summary>
        /// <param name="feedType">Type of the feed.</param>
        /// <param name="username">The username.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="fromid">The from id.</param>
        /// <returns>X Container.</returns>
        private XContainer GetXmlFeeds(Type feedType, string username, string timestamp, string domain, int? fromid)
        {
            var ip = Utilities.GetIpAddressOfClient();

            var err = IsAuthenticated(username, timestamp, domain, ip);

            if (err.HasValue && err.Value.Id == 0)
            {
                ////try
                ////{
                Type genericType = typeof(Feeds<>);
                Type a = genericType.MakeGenericType(feedType);
                dynamic obj = Activator.CreateInstance(a);

                return obj.BuildXmlDocument(domain, ip, username, fromid);
                ////}
                ////catch (Exception e)
                ////{
                ////    Helper.LogError(e.Message);
                ////    string errMsg = "Sorry for the inconvenience! We are not able to process your request this time. Please try again later";
                ////    return new Error(4, errMsg).ToXElement();
                ////}
            }

            return err.Value.ToXElement();
        }

        /// <summary>
        /// Determines whether the specified username is authenticated.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="ip">The IP to authenticate.</param>
        /// <returns>Class Error.</returns>
        private Error? IsAuthenticated(string username, string timestamp, string domain, string ip)
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
