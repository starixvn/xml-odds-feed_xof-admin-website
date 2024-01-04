namespace Fanex.OddsFeed.Criteria.Licensee
{
    using Fanex.Data;

    public class EditLicenseeInfoCriteria : BaseNonQueryCommand
    {
        public short SystemID { get; set; }

        public int LicenseeID { get; set; }

        public bool IsActive { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public string ContactEmail { get; set; }

        [SpParam(Ignored = true)]
        public string SystemName { get; set; }

        [SpParam(Ignored = true)]
        public bool AddExistingLicensee { get; set; }

        public override string GetSettingKey() => "Odds_LicenseeInfoUpd";

        public override bool IsValid() => true;
    }
}