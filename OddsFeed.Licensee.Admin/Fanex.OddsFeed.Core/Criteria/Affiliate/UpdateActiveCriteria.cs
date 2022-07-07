namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class UpdateActiveCriteria : CriteriaBase
    {
        public int PartnerID { get; set; }

        public bool IsAllowed { get; set; }

        public int Creator { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Odds_Partner_Upd_Active";

        public override bool IsValid() => true;
    }
}