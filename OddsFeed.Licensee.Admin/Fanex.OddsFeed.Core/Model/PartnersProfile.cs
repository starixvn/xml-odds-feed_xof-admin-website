// --------------------------------------------------------------------------
// <copyright file="PartnersProfile.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.08.22">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Web;

    /// <summary>
    /// Class PartnersProfile.
    /// </summary>
    [Serializable]
    public class PartnersProfile
    { 
        public string AffsOddsType { get; set; }

        ////public string AffsOddsRegion { get; set; }

        public string AffsLanguageList { get; set; }

        public string AffsSportIDList { get; set; }

        public string AffsCountryCodeList { get; set; }

        public string AffsLeagueIDList { get; set; }

        public string AffsOutrightIDList { get; set; }

        public string AffsBettypeIDList { get; set; }
    }
}