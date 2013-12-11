// --------------------------------------------------------------------------
// <copyright file="PermissionFlag.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.01.25">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model
{
    using System;
    using System.Diagnostics.CodeAnalysis;    
    using System.Xml.Serialization;

    /// <summary>
    /// Class PermissionFlag.
    /// </summary>
    [Serializable]
    public class PermissionFlag
    {
        public short NumberOfIPs
        {
            get;
            set;
        }

        public bool OddsTypeFlag
        {
            get;
            set;
        }

        public bool OddsRegionFlag
        {
            get;
            set;
        }

        public bool LanguageListFlag
        {
            get;
            set;
        }

        public bool SportIDListFlag
        {
            get;
            set;
        }

        public bool CountryCodeListFlag
        {
            get;
            set;
        }

        public bool BetTypeIDListFlag
        {
            get;
            set;
        }
    }
}