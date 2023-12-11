namespace Fanex.OddsFeed.Criteria.Licensee
{
    using Data.Repository;

    public class EditLicenseeCriteria : BaseNonQueryCommand
    {
        public short SystemID { get; set; }

        public string SystemName { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey() => "Odds_SystemUpd";

        public override bool IsValid() => true;
    }
}