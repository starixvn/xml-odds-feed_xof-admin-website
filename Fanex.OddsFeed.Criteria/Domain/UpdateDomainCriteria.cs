namespace Fanex.OddsFeed.Criteria.Domain
{
    using Data.Repository;

    public class UpdateDomainCriteria : BaseNonQueryCommand
    {
        public short DomainID { get; set; }

        public string DomainName { get; set; }

        public string IP { get; set; }

        public string ISP { get; set; }

        public int Creator { get; set; }

        public string CurrentIP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Odds_DomainUpd";

        public override bool IsValid() => true;
    }
}