namespace Fanex.OddsFeed.Core.Criteria.CompanyIP
{
    using Data.Repository;

    public class UpdateCompanyIPCriteria : BaseCommand
    {
        public int Creator { get; set; }

        public string IPFrom { get; set; }

        public string IPTo { get; set; }

        public string IPFromOld { get; set; }

        public string IPToOld { get; set; }

        public string Description { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Odds_CompanyIP_Setting";

        public override bool IsValid() => true;
    }
}
