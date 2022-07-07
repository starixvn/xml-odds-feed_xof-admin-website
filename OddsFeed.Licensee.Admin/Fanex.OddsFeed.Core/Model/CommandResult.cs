namespace Fanex.OddsFeed.Core.Model
{
    public class CommandResult
    {
        public dynamic Code { get; set; }
        public string Message { get; set; }
        public bool IsSuccessful => Code == 0;
    }
}