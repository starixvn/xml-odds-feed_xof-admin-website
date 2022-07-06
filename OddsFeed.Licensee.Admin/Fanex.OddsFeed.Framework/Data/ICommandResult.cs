namespace Fanex.OddsFeed.Framework.Data
{
    public interface ICommandResult
    {
        dynamic Code { get; }

        dynamic Message { get; }

        bool IsSuccessful { get; }
    }
}