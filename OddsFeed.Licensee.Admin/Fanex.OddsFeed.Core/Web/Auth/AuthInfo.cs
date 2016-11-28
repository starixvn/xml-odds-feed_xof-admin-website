// --------------------------------------------------------------------------
// <copyright file="AuthInfo.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Web.Auth
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Class AUTHInfo.
    /// </summary>    
    public class AuthInfo
    {
        private int _userId;

        private int _onuserId;

        public int UserId
        {
            get 
            { 
                return _userId; 
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("userid");
                }

                _userId = value;
            }
        }
     
        public int OnUserId
        {
            get 
            { 
                return _onuserId; 
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("onUserid");
                }

                _onuserId = value;
            }
        }

        public static AuthInfo CreateUserData(string authInfoXml)
        {
            var serializer = new XmlSerializer(typeof(AuthInfo));
            using (var stringReader = new StringReader(authInfoXml))
            {
                var authInfo = (AuthInfo)serializer.Deserialize(stringReader);
                return authInfo;
            }
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns>Object System.String.</returns>
        public string ToXml()
        {
            var xml = string.Empty;
            using (StringWriter sw = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(AuthInfo));
                serializer.Serialize(sw, this);
                sw.Close();
                xml = sw.ToString();
            }

            return xml;
        }     
    }
}