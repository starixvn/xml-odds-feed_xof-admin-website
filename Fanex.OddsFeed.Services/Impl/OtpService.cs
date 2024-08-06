namespace Fanex.OddsFeed.Services
{
    using System;
    using Fanex.OddsFeed.Models;
    using Google.Authenticator;

    public class OtpService(OtpConfig config) : IOtpService
    {
        public string GenerateSetupCode(string userId, string secretKey)
        {
            var authenticator = new TwoFactorAuthenticator();
            var setupInfo = authenticator.GenerateSetupCode(config.AppName, userId, secretKey, false);
            return setupInfo.ManualEntryKey;
        }

        public bool ValidateOTPCode(string secretKey, string otpCode)
        {
            var timeExpireOTP = TimeSpan.FromSeconds(config.OtpExpireTime);
            var authenticator = new TwoFactorAuthenticator();
            return authenticator.ValidateTwoFactorPIN(secretKey, otpCode, timeExpireOTP);
        }
    }
}
