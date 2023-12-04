namespace Fanex.OddsFeed.Core.Context
{
    public interface ICurrentContext
    {
        string IP { get; }

        string DomainName { get; }
    }
}
