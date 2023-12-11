namespace Fanex.OddsFeed.Criteria.Domain
{
    using Data.Repository;

    public class DeleteDomainCriteria : BaseNonQueryCommand
    {
        public short DomainID { get; set; }

        public int Creator { get; set; }

        public string Ip { get; set; }

        public override string GetSettingKey() => "Odds_DomainDel";

        public override bool IsValid() => true;
    }
}