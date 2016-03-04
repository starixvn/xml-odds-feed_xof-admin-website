// --------------------------------------------------------------------------
// <copyright file="UpdateProfileCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.01.09">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class UpdateProfileCriteria.
    /// </summary>    
    public class UpdateProfileCriteria : Framework.Data.CriteriaBase
    {
        public UpdateProfileCriteria()
        {            
        }

        public int PartnerID { get; set; }

        public int Creator { get; set; }

        public string OddsType { get; set; }

        public string LanguageList { get; set; }

        public string OddsRegion { get; set; }

        public string SportIDList { get; set; }

        public string CountryCodeList { get; set; }     

        public string BetTypeIDList { get; set; }

        public string LeagueIDList { get; set; }

        public string OutRightIDList { get; set; }

        public string LeagueKeyword { get; set; }

        public string LeagueKeyword_O { get; set; }        

        [SpParam(Ignored = true)]
        public string DeadballDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string LiveDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string OutrightDelayTimes { get; set; }

        public string DelayTimes
        {
            get
            {
                return string.Join(",", this.DeadballDelayTimes, this.LiveDelayTimes, this.OutrightDelayTimes);
            }
        }

        public short NumberOfIPs { get; set; }

        #region For Licensee Admin/Sub-Acc
        
        public bool OddsTypeFlag { get; set; }

        public bool OddsRegionFlag { get; set; }
        
        public bool LanguageListFlag { get; set; }

        public bool SportIDListFlag { get; set; }

        public bool CountryCodeListFlag { get; set; }

        public bool BetTypeIDListFlag { get; set; }

        #endregion

        public override string GetSettingKey()
        {
            return "Odds_Partner_Upd_Profile";
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