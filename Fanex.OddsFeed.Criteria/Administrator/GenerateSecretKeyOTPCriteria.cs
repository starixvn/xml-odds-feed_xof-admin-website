namespace Fanex.OddsFeed.Criteria.Administrator
{
    public class GenerateSecretKeyOTPCriteria : BaseNonQueryCommand
    {
        public int UserId { get; set; }
        public string SecretKey { get; set; }
        public int Creator { get; set; }
        public string IP { get; set; }

        public override string GetSettingKey() => "Odds_OTP_AdminUserOTPIns";

        public override bool IsValid() => UserId > 0 && Creator > 0;
    }
}
