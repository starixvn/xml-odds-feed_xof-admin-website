namespace Fanex.OddsFeed.Core.Command
{
    using Fanex.OddsFeed.Framework.Data;

    public class CommandResult : CommandResultBase
    {
        public override bool IsSuccessful => Code == 0;
    }
}