namespace Fanex.OddsFeed.Criteria.Administrator
{
    using System.ComponentModel.DataAnnotations;
    public class GenerateSecretKeyOTPCriteria : BaseNonQueryCommand
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string SecretKey { get; set; }

        [Required]
        public int Creator { get; set; }

        [Required]
        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_OTP_AdminUserOTPIns";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
