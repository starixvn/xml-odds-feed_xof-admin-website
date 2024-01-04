namespace Fanex.OddsFeed.Criteria.CompanyIP
{
    public class UpdateCompanyIPCriteria : BaseNonQueryCommand
    {
        public int Creator { get; set; }

        public string IPFrom { get; set; }

        public string IPTo { get; set; }

        public string IPFromOld { get; set; }

        public string IPToOld { get; set; }

        public string Description { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey() => "Odds_CompanyIP_Setting";

        public override bool IsValid() => true;
    }
}
