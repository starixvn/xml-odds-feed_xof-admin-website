namespace Fanex.OddsFeed.Core.Model
{
    public class OddsFeedActionResult
    {
        public int ErrorCode { get; set; }

        public bool IsSuccess { get; set; } = true;

        public string ResultMessage { get; set; } = string.Empty;
    }
}