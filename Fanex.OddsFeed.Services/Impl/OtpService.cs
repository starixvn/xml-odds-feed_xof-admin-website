namespace Fanex.OddsFeed.Services.Impl
{
    using System;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Services.Interface;
    using Google.Authenticator;
    public class OtpService : IOtpService
    {
        private readonly OtpConfig config;

        public OtpService(OtpConfig config)
        {
            this.config = config;
        }

        public string GenerateSetupCode(string userId, string secretKey)
        {
            var twoFactorAuthen = new TwoFactorAuthenticator();
            var setupInfo = twoFactorAuthen.GenerateSetupCode(config.AppName, userId, secretKey, false);
            return setupInfo.ManualEntryKey;
        }

        public bool ValidateOTPCode(string secretKey, string otpCode)
        {
            var timeExpireOTP = TimeSpan.FromSeconds(config.OtpExpireTime);
            var twoFactorAuthenticator = new TwoFactorAuthenticator();
            var validOTP = twoFactorAuthenticator.ValidateTwoFactorPIN(secretKey, otpCode, timeExpireOTP);

            return validOTP;
        }
    }
}
