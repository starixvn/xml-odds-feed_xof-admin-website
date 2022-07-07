namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.OddsFeed.Core.Web;
    using Data.Repository;

    public class UpdateAffiliateCriteria : CriteriaBase
    {
        public int PartnerID { get; set; }

        [Required]
        [Display(Name = "Website", ResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "Site")]
        public string Website { get; set; }

        [Display(Name = "Note", ResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "Remark")]
        public string Note { get; set; }

        public int UpdatedBy { get; set; }

        [Required]
        [Display(Name = "Email", ResourceType = typeof(ViewResources))]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
           ErrorMessageResourceName = "EmailFormatValidationMsg",
           ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(SqlParamName = "ContactEmail")]
        public string Email { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; } = string.Empty;


        // Ignored
        [Required]
        [Display(Name = "Name", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidNameValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(Ignored = true)]
        public string UserName { get; set; }

        public override string GetSettingKey() => "Odds_Partner_Upd_Info";

        public override bool IsValid() => true;
    }
}