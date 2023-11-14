namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;
    using Fanex.Data;

    public class UpdateSettingCriteria : BaseCommand
    {
        public int PartnerID { get; set; }

        [SpParam(SqlParamName = "ListDomainId")]
        public string Domains { get; set; }

        [SpParam(SqlParamName = "ListIPFrom")]
        public string IpsFrom { get; set; }

        [SpParam(SqlParamName = "ListIPTo")]
        public string IpsTo { get; set; }

        public override string GetSettingKey() => "Odds_Partner_Upd_Setting";

        public override bool IsValid() => true;
    }
}