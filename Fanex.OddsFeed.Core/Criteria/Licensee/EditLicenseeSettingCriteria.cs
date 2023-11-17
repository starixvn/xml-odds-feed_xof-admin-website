namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using Data.Repository;
    using Fanex.Data;

    public class EditLicenseeSettingCriteria : BaseCommand
    {
        public short SystemID { get; set; }

        public string DomainList { get; set; }

        public string ListIPFrom { get; set; }

        public string ListIPTo { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        [SpParam(Ignored = true)]
        public string SystemName { get; set; }

        public override string GetSettingKey() => "Odds_LicenseeSettingUpd";

        public override bool IsValid() => true;
    }
}