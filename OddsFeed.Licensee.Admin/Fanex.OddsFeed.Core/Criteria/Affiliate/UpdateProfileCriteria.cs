// --------------------------------------------------------------------------
// <copyright file="UpdateProfileCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.01.09">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    /// <summary>
    /// Class UpdateProfileCriteria.
    /// </summary>
    public class UpdateProfileCriteria : CriteriaBase
    {
        public UpdateProfileCriteria()
        {
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int Creator { get; set; }

        public int PartnerID { get; set; }

        public string OddsType { get; set; }

        public string OddsRegion { get; set; }

        public string LanguageList { get; set; }

        public string SportIDList { get; set; }

        public string BetTypeIDList { get; set; }

        public string LeagueIDList { get; set; }

        public string CountryCodeList { get; set; }

        public string LeagueKeyword { get; set; }

        public string LeagueKeyword_O { get; set; }

        public string DelayTimes
        {
            get
            {
                return string.Join(",", this.DeadballDelayTimes, this.LiveDelayTimes, this.OutrightDelayTimes);
            }
        }

        public string OutRightIDList { get; set; }

        public short NumberOfIPs { get; set; }

        public bool OddsTypeFlag { get; set; }

        public bool LanguageListFlag { get; set; }

        public bool OddsRegionFlag { get; set; }

        public bool SportIDListFlag { get; set; }

        public bool BetTypeIDListFlag { get; set; }

        public bool CountryCodeListFlag { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        // Ignored
        [SpParam(Ignored = true)]
        public string DeadballDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string LiveDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string OutrightDelayTimes { get; set; }

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