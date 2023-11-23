namespace Fanex.OddsFeed.Core.Context
{
    public interface ICurrentContext
    {
        string IP { get; }

        string IP4Address { get; }

        string IP6Address { get; }

        string DomainName { get; }

        string BrowserName { get; }

        string Platform { get; }

        string CountryCode { get; }
    }
}
