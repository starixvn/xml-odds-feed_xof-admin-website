namespace Fanex.OddsFeed.Library.Services
{
    public interface IWindowsOddsFeedService
    {
        void Reset();

        bool StoreFullChain();

        bool Update(int currentVersion);
    }
}
