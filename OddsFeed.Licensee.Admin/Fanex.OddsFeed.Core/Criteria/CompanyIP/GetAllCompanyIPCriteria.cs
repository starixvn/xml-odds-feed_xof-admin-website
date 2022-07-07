namespace Fanex.OddsFeed.Core.Criteria.CompanyIP
{
    using Data.Repository;

    public class GetAllCompanyIPCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Odds_CompanyIPSel";

        public override bool IsValid() => true;
    }
}