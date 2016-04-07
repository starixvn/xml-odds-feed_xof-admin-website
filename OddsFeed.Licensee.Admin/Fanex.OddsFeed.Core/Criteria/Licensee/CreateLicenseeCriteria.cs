// --------------------------------------------------------------------------
// <copyright file="CreateLicenseeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.3.6">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.OddsFeed.Core.Web;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class CreateLicenseeCriteria.
    /// </summary>
    public class CreateLicenseeCriteria : CriteriaBase
    {
        public CreateLicenseeCriteria()
        {
            this.SystemName = string.Empty;            
            this.LicenseeID = 0;
            this.IsActive = true;
            this.ContactEmail = string.Empty;
            this.DomainList = string.Empty;
            this.ListIPFrom = string.Empty;
            this.ListIPTo = string.Empty;
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

        [Required]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidNameValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string SystemName { get; set; }
      
        public int LicenseeID { get; set; }

        public bool IsActive { get; set; }

        public string DomainList { get; set; }

        public string ListIPFrom { get; set; }

        public string ListIPTo { get; set; }

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

        public string OutRightIDList { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        [Required]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessageResourceName = "EmailFormatValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string ContactEmail { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }


        // Ignored
        [SpParam(Ignored = true)]
        public bool AddExistingLicensee { get; set; }

        [SpParam(Ignored = true)]
        public string DeadballDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string LiveDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string OutrightDelayTimes { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_LicenseeIns";
        }
    }
}
