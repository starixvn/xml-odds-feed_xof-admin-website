namespace Fanex.OddsFeed.Models
{
    public class CustomLoginResult
    {
        public int Code { get; set; } = 1;
        public string Message { get; set; }
        public bool IgnoreOTP { get; set; }
        public bool IsSuccessful => Code == 0;
    }
}
