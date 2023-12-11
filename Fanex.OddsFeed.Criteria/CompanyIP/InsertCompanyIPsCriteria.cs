namespace Fanex.OddsFeed.Criteria.CompanyIP
{
    using Data.Repository;

    public class InsertCompanyIPsCriteria : BaseNonQueryCommand
    {
        public string ListIPFrom { get; set; }

        public string ListIPTo { get; set; }

        public string ListDescription { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey() => "Odds_CompanyIPIns";

        public override bool IsValid() => true;
    }
}
