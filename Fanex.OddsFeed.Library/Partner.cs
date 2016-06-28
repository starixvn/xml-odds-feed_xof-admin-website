// --------------------------------------------------------------------------
// <copyright file="Partner.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Library
{
    using Fanex.Common;
    using Fanex.Data.DNAide;
    using System;
    using System.Collections;
    using System.Xml;

    /// <summary>
    /// Class Partner.
    /// </summary>
    public class Partner
    {
        private string _name;
        private string _domainName;
        private string _ip;
        private string _queryTimestamp;
        private string _storeProcedureOutputTimestamp;
        private string _systemName;
        private XmlDocument _xdoc;
        private OddsProfile _op;

        public Partner(string username, string domainName, string ip, string timestamp)
        {
            _name = username;
            _domainName = domainName;
            _ip = ip;
            _queryTimestamp = timestamp;
            _systemName = string.Empty;
            _xdoc = new XmlDocument();
            var xmlDeclaration = _xdoc.CreateXmlDeclaration("1.0", "utf-8", null);
            _xdoc.InsertBefore(xmlDeclaration, _xdoc.DocumentElement);
        }

        public string Timestamp
        {
            get
            {
                return _storeProcedureOutputTimestamp;
            }
        }

        public OddsProfile OddsProfile
        {
            get
            {
                return _op;
            }
        }

        public string SystemName
        {
            get
            {
                return _systemName;
            }
        }

        /// <summary>
        /// Determines whether this instance is authenticated.
        /// </summary>
        /// <returns>The object error.</returns>
        public Error IsAuthenticated()
        {
            ////int profileId = 0;
            var hash = new Hashtable
                             {
                                 { "username", _name },
                                 { "domainname", _domainName },
                                 { "ip", _ip },
                                 { "SystemName", "" },
                                 { "ErrCode", 0 },
                                 { "ErrMsg", "" }
                             };
            try
            {
                var ret = DbHelper.ExecuteDbResult(Utilities.RealSp("SP.CheckPermission"), hash);

                if (Converter.ToInt(ret.Outputs["errcode"]) != 0)
                {
                    return new Error(Converter.ToInt(ret.Outputs["errcode"]), Converter.ToString2(ret.Outputs["errmsg"]));
                }

                _systemName = Converter.ToString2(ret.Outputs["systemname"]);
                ////Converter.ToString2(ret.Outputs["spname"]);                
                _storeProcedureOutputTimestamp = Converter.ToString2(ret.Outputs["timestamp"]);
                ////Converter.ToInt(ret.Outputs["cachetime"]);
                ////profileId = Converter.ToInt(ret.Outputs["profileid"]);

                if (ret.ResultSet.Tables.Count > 0)
                {
                    _op = new OddsProfile(ret);
                }

                return new Error(0, string.Empty);
            }
            catch (Exception e)
            {
                return new Error(10000, e.Message);
            }
        }
    }
}
