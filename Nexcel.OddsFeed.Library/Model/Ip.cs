// --------------------------------------------------------------------------
// <copyright file="Ip.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Nexcel.OddsFeed.Library.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class IP (internet protocol).
    /// </summary>
    [Serializable]
    public class Ip : BaseModel
    {
        public int IpKeyFrom { get; set; }

        public int IpKeyTo { get; set; }
        
        public long IpFrom { get; set; }

        public long IpTo { get; set; }

        public string CountryShort { get; set; }

        public string CountryLong { get; set; }

        public string IpRegion { get; set; }

        public string IpCity { get; set; }

        public bool IsModified { get; set; }

        public bool IsException { get; set; }
    }
}
