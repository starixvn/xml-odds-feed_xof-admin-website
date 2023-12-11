namespace Fanex.OddsFeed.Criteria.CompanyIP
{
    using Data.Repository;

    public class DeleteCompanyIPCriteria : BaseNonQueryCommand
    {
        public int Creator { get; set; }

        public string IP { get; set; }

        public string IPFromOld { get; set; }

        public string IPToOld { get; set; }

        public override string GetSettingKey() => "Odds_CompanyIP_DeleteSetting";

        public override bool IsValid() => true;
    }
}
