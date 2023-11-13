namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    public class UpdateLicenseeActiveCriteria : ResultCriteria
    {
        public short SystemID { get; set; }

        public bool IsActive { get; set; }

        public bool IsActiveAllAffiliate { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey() => "Odds_LicenseeActiveUpd";

        public override bool IsValid() => true;
    }
}