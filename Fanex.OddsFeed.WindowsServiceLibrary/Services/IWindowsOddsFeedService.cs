namespace Fanex.OddsFeed.WindowsServiceLibrary.Services
{
    using System;

    public interface IWindowsOddsFeedService
    {
        void Reset();

        bool StoreFullChain();

        bool Update(int currentVersion);
        
        TimeSpan FullTime { get; set; }

        TimeSpan LiveballTime { get; set; }

        TimeSpan DeadballTime { get; set; }

        TimeSpan OutrightTime { get; set; }
    }
}
