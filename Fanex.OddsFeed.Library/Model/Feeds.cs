// --------------------------------------------------------------------------
// <copyright file="Feeds.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Library.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;    
    using System.Reflection;
    using System.Xml.Linq;    
    using Fanex.OddsFeed.Library;
    using Fanex.OddsFeed.Library.Model;    
    using Fanex.OddsFeed.Library.Parsers;

    /// <summary>
    /// Class Feeds.
    /// </summary>
    /// <typeparam name="T">Object reference.</typeparam>
    public class Feeds<T> where T : BaseModel
    {        
        private IList<T> _feeds;        
        ////private string _timestamp;
  
        public Feeds()
        {           
            ////get profile
            //// op = new OddsProfile(DbConnect.GetProfile(profileId));
            //// op = profile;            
        }

        public XContainer BuildXmlDocument(string domainName, string ip, string username, int? fromid = null)
        {
            var rootName = LoadData(domainName, ip, username, fromid);

            return ORMParsers.BuildXDocument(rootName, _feeds);
        }

        public string BuildJson(string domainName, string ip, string username, int? fromid = null)
        {
            var rootName = LoadData(domainName, ip, username, fromid);

            return ORMParsers.BuildJSon(rootName, _feeds);
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="ip">The accessing IP.</param>
        /// <param name="username">The username.</param>
        /// <param name="fromid">The from id.</param>
        /// <returns>System String.</returns>
        /// <exception cref="System.Exception">Feeds Type T BuildXmlDocument(): This generic type T is not a subclass of BaseModel.</exception>
        private string LoadData(string domainName, string ip, string username, int? fromid)
        {
            var t = typeof(T);
            var tableName = t.Name;                       

            var db = DbConnect.GetOcsData(tableName, domainName, ip, username, fromid);

            _feeds = ORMParsers.TableToList<T>(db.ResultSet.Tables[0]);

            var prop = t.GetProperty("PluralName", BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (prop == null)
            {
                throw new Exception("Feeds<T>.BuildXmlDocument(): This generic type <T> is not a subclass of BaseModel");
            }

            var rootName = prop.GetValue(null, null).ToString();
            return rootName;
        }
    }
}
