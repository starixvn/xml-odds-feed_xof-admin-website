namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    public class DeleteAffiliateCriteria : CriteriaBase
    {
        public int PartnerID { get; set; }

        public int Creator { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Odds_Partner_Del";

        public override bool IsValid() => true;
    }
}