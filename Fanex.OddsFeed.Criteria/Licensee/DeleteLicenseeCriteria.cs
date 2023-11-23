namespace Fanex.OddsFeed.Criteria.Licensee
{
    using Data.Repository;

    public class DeleteLicenseeCriteria : BaseCommand
    {
        public short SystemID { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Odds_LicenseeDel";

        public override bool IsValid() => true;
    }
}