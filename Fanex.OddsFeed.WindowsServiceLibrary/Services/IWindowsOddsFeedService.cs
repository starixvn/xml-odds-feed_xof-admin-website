namespace Fanex.OddsFeed.WindowsServiceLibrary.Services
{
    public interface IWindowsOddsFeedService
    {
        void Reset();

        bool StoreFullChain();

        bool Update(int currentVersion);
    }
}
