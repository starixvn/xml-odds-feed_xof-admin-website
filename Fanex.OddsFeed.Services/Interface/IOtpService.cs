namespace Fanex.OddsFeed.Services
{
    public interface IOtpService
    {
        string GenerateSetupCode(string userId, string secretKey);

        bool ValidateOTPCode(string secretKey, string otpCode);
    }
}
