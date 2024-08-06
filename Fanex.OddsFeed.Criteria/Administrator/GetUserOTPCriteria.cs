namespace Fanex.OddsFeed.Criteria.Administrator
{
    using Fanex.Data.Repository;

    public class GetUserOTPCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public override string GetSettingKey() => "Odds_OTP_AdminUserOTPSel";

        public override bool IsValid() => UserId > 0;
    }
}
