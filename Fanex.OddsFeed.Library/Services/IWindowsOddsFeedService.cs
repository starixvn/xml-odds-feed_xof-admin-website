namespace Fanex.OddsFeed.Library.Services
{
    public interface IWindowsOddsFeedService
    {
        bool Reset();

        bool StoreFullChain();

        bool Update(int currentVersion);
    }
}
