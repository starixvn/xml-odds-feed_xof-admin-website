namespace Fanex.OddsFeed.Core.Web.Context
{
    public interface ICurrentContext
    {
        string IP { get; }

        string IP4Address { get; }

        string IP6Address { get; }

        string DomainName { get; }

        string FullDomainName { get; }

        string BrowserName { get; }

        string Platform { get; }

        string CountryCode { get; }
    }
}