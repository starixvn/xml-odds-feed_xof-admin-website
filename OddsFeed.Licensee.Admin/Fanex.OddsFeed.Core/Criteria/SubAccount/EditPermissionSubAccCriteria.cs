namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.OddsFeed.Core.Web;
    using Data.Repository;

    public class EditPermissionSubAccCriteria : ResultCriteria
    {
        [SpParam(SqlParamName = "UserID")]
        public int SubAccountID { get; set; }

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

        public int UpdatedBy { get; set; }

        [SpParam(Ignored = true)]
        public string SubAccountName { get; set; }

        public override string GetSettingKey() => "Odds_User_EditPermission";

        public override bool IsValid() => true;
    }
}