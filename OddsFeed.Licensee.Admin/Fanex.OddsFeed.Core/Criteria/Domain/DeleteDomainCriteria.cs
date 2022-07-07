namespace Fanex.OddsFeed.Core.Criteria.Domain
{
    using Data.Repository;

    public class DeleteDomainCriteria : CriteriaBase
    {
        public short DomainID { get; set; }

        public int Creator { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Odds_DomainDel";

        public override bool IsValid() => true;
    }
}