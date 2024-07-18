namespace Fanex.OddsFeed.Criteria.Administrator
{
    using System.ComponentModel.DataAnnotations;
    public class UpdateIgnoreOTPCriteria : BaseNonQueryCommand
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public bool IsIgnoreOTP { get; set; }

        [Required]
        public int Creator { get; set; }

        [Required]
        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_OTP_AdminUserOTP_IgnoreOTP";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
