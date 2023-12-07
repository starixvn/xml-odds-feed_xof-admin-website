namespace Fanex.OddsFeed.Criteria.Affiliate
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.Data;
    using Fanex.OddsFeed.Resources;

    public class InsertAffiliateCriteria : BaseNonQueryCommand
    {
        public int PartnerID { get; set; }

        public int SystemID { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidNameValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Website", ResourceType = typeof(ViewResources))]
        [RegularExpression(@"^http\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$",
            ErrorMessageResourceName = "InvalidURLValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "Site")]
        public string Website { get; set; }

        [Display(Name = "Note", ResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "Remark")]
        public string Note { get; set; }

        public int Creator { get; set; }

        public string ContactPerson { get; set; }

        [Required]
        [Display(Name = "Email", ResourceType = typeof(ViewResources))]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessageResourceName = "EmailFormatValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "ContactEmail")]
        public string Email { get; set; }
        
        public string Content { get; set; }
        
        [SpParam(SqlParamName = "ListDomainId")]
        public string Domains { get; set; }

        [SpParam(SqlParamName = "ListIPFrom")]
        public string IpsFrom { get; set; }

        [SpParam(SqlParamName = "ListIPTo")]
        public string IpsTo { get; set; }

        public string OddsType { get; set; }

        public string OddsRegion { get; set; }

        public string LanguageList { get; set; }

        public string SportIDList { get; set; }

        public string BetTypeIDList { get; set; }

        public string CountryCodeList { get; set; }

        public string LeagueIDList { get; set; }

        public string LeagueKeyword { get; set; }

        public string LeagueKeyword_O { get; set; }

        public string OddsDelayTimes { get; set; }

        public string OutRightIDList { get; set; }

        public short NumberOfIPs { get; set; }

        public bool OddsTypeFlag { get; set; }

        public bool OddsRegionFlag { get; set; }

        public bool LanguageListFlag { get; set; }

        public bool SportIDListFlag { get; set; }

        public bool CountryCodeListFlag { get; set; }

        public bool BetTypeIDListFlag { get; set; }

        public string Ip { get; set; }
        
        [SpParam(Ignored = true)]
        public string DeadballDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string LiveDelayTimes { get; set; }

        [SpParam(Ignored = true)]
        public string OutrightDelayTimes { get; set; }

        public string ResultDelayTimes { get; set; }

        public override string GetSettingKey() => "Odds_Partner_Ins";

        public override bool IsValid() => true;
    }
}