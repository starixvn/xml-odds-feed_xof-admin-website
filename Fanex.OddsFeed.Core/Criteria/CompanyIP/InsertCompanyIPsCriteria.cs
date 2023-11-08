namespace Fanex.OddsFeed.Core.Criteria.CompanyIP
{
    using Data.Repository;

    public class InsertCompanyIPsCriteria : BaseCommand
    {
        public string ListIPFrom { get; set; }

        public string ListIPTo { get; set; }

        public string ListDescription { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Odds_CompanyIPIns";

        public override bool IsValid() => true;
    }
}
