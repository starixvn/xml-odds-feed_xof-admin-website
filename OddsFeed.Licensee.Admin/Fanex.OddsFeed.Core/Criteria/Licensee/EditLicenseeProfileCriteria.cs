// --------------------------------------------------------------------------
// <copyright file="EditLicenseeProfileCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.3.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using System;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class EditLicenseeProfileCriteria.
    /// </summary>
    public class EditLicenseeProfileCriteria : CriteriaBase
    {
        public EditLicenseeProfileCriteria()
        {
            this.SystemID = 0;
            this.Editable = true;
            this.Editable = true;
            this.OddsType = string.Empty;
            this.LanguageList = string.Empty;
            this.OddsRegion = string.Empty;
            this.SportIDList = string.Empty;
            this.BetTypeIDList = string.Empty;
            this.LeagueIDList = string.Empty;
            this.OutRightIDList = string.Empty;
            this.CountryCodeList = string.Empty;
            this.LeagueKeyword = string.Empty;
            this.LeagueKeyword_O = string.Empty;
            this.DeadballDelayTimes = string.Empty;
            this.LiveDelayTimes = string.Empty;
            this.OutrightDelayTimes = string.Empty;
            this.NumberOfIPs = 1;
            this.OddsTypeFlag = true;
            this.LanguageListFlag = true;
            this.OddsRegionFlag = true;
            this.SportIDListFlag = true;
            this.BettypeIDListFlag = true;
            this.LeagueIDListFlag = true;
            this.CountryCodeListFlag = true;
            this.LeagueKeywordFlag = true;
            this.LeagueKeyword_OFlag = true;
            this.DelayTimesFlag = true;
        }
        
        
        
        public short SystemID { get; set; }
        
        public bool Editable { get; set; }

        public string OddsType { get; set; }

        public string LanguageList { get; set; }

        public string OddsRegion { get; set; }

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

        public bool BettypeIDListFlag { get; set; }

        public bool LeagueIDListFlag { get; set; }

        public bool CountryCodeListFlag { get; set; }

        public bool LeagueKeywordFlag { get; set; }

        public bool LeagueKeyword_OFlag { get; set; }

        public bool DelayTimesFlag { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public bool ApplyAllCurrentAffiliates { get; set; }

        public bool IsForceUpdate { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }


        // Ignored
        [SpParam(Ignored = true)]
        public string LeagueIDOutRightIDList
        {
            get
            {
                return string.Join(";", this.LeagueIDList, this.OutRightIDList);
            }
        }

        [SpParam(Ignored = true)]
        public string DeadballDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string LiveDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string OutrightDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string SystemName { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_LicenseeProfileUpd";
        }
    }
}
