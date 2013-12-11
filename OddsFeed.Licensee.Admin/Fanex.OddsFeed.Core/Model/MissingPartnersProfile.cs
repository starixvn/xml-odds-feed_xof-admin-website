// --------------------------------------------------------------------------
// <copyright file="MissingPartnersProfile.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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
    /// Class MissingPartnersProfile.
    /// </summary>
    [Serializable]
    public class MissingPartnersProfile
    {
        public MissingPartnersProfile()
        {
            this.ErrCode = 0;
    
            this.AffsOddsType = string.Empty;
            this.AffsLanguageList = string.Empty;
            this.AffsLanguageList = string.Empty;
            this.AffsSportIDList = string.Empty;
            this.AffsCountryCodeList = string.Empty;
            this.AffsLeagueIDList = string.Empty;
            this.AffsOutrightIDList = string.Empty;
            this.AffsBettypeIDList = string.Empty;
        }

        public int ErrCode { get; set; }
        
        public string AffsOddsType { get; set; }        

        public string AffsLanguageList { get; set; }

        public string AffsSportIDList { get; set; }

        public string AffsCountryCodeList { get; set; }

        public string AffsLeagueIDList { get; set; }

        public string AffsOutrightIDList { get; set; }

        public string AffsBettypeIDList { get; set; }
    }
}