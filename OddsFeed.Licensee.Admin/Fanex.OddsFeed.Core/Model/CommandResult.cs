namespace Fanex.OddsFeed.Core.Model
{
    public class CommandResult
    {
        public int Code { get; set; } = 0;
        public string Message { get; set; }
        public bool IsSuccessful => Code == 0;
    }
}