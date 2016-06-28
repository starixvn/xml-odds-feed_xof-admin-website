// --------------------------------------------------------------------------
// <copyright file="GetProfileAllPartnersBySystemIdCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.08.22">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using System.Diagnostics.CodeAnalysis;
    using Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class GetProfileAllPartnersBySystemIdCriteria.
    /// </summary>
    public class GetProfileAllPartnersBySystemIdCriteria : CriteriaBase
    {
        public GetProfileAllPartnersBySystemIdCriteria()
        {
            ErrCode = 0;
            ErrMsg = string.Empty;
        }
        
        public int SystemID { get; set; }

        public string AffsOddsType { get; set; }
        public string AffsLanguageList { get; set; }
        public string AffsSportIDList { get; set; }
        public string AffsCountryCodeList { get; set; }
        public string AffsLeagueIDList { get; set; }
        public string AffsOutrightIDList { get; set; }
        public string AffsBettypeIDList { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>String "Odds_GetProfileAllPartnersBySystemId".</returns>
        public override string GetSettingKey()
        {
            return "Odds_GetProfileAllPartnersBySystemId";
        }

        /// <summary>
        /// Check data's constrains.
        /// </summary>
        /// <returns>True: valid; False: invalid.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
