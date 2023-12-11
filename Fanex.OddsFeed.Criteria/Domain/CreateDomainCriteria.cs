namespace Fanex.OddsFeed.Criteria.Domain
{
    using Data.Repository;

    public class CreateDomainCriteria : BaseNonQueryCommand
    {
        public short DomainID { get; set; }

        public string DomainName { get; set; }

        public string IP { get; set; }

        public string ISP { get; set; }

        public int Creator { get; set; }

        public string CurrentIP { get; set; }

        public override string GetSettingKey() => "Odds_DomainIns";

        public override bool IsValid() => true;
    }
}
