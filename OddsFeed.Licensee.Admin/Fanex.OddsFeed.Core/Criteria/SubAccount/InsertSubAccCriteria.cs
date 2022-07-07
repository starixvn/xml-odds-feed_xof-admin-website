namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Fanex.OddsFeed.Core.Web;
    using Data.Repository;

    public class InsertSubAccCriteria : ResultCriteria
    {
        public int UserID { get; set; }

        public int SystemID { get; set; }

        public byte RoleID { get; set; }

        public int Creator { get; set; }

        public string Ip { get; set; }

        #region step1

        [Required]
        [Display(Name = "Name", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidNameValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 6,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string Password { get; set; }

        [Display(Name = "First_Name", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string FirstName { get; set; }

        [Display(Name = "Last_Name", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email", ResourceType = typeof(ViewResources))]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessageResourceName = "EmailFormatValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Is_Active", ResourceType = typeof(ViewResources))]
        public bool IsActive { get; set; }

        #endregion

        #region Step2

        [Required]
        [SpParam(SqlParamName = "SystemIDList")]
        public string Systems { get; set; }
        
        #endregion

        #region step3

        [Display(Name = "Odds_type", ResourceType = typeof(ViewResources))]
        public bool OddsTypeFlag { get; set; }

        [Display(Name = "Odds_format", ResourceType = typeof(ViewResources))]
        public bool OddsStyleFlag { get; set; }

        [Display(Name = "Languages", ResourceType = typeof(ViewResources))]
        public bool LanguageListFlag { get; set; }

        [Display(Name = "Sports", ResourceType = typeof(ViewResources))]
        public bool SportIDListFlag { get; set; }

        [Display(Name = "Countries", ResourceType = typeof(ViewResources))]
        public bool CountryCodeListFlag { get; set; }

        [Display(Name = "Markets", ResourceType = typeof(ViewResources))]
        public bool BetTypeIDListFlag { get; set; }

        #endregion

        [SpParam(Ignored = true)]
        public int SubAccountID { get; set; }

        [SpParam(Ignored = true)]
        public string[] SystemArray
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Systems))
                {
                    return null;
                }

                return Systems.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            set
            {
                Systems = Utilities.StringArrayToString(value);
            }
        }

        public override string GetSettingKey() => "Odds_User_Ins";

        public override bool IsValid() => true;
    }
}