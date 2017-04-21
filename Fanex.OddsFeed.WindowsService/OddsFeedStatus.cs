namespace Fanex.OddsFeed.WindowsService
{
    public static class OddsFeedStatus
    {
        public static int CurrentVersion { get; set; }

        public static bool IsOddsFeedRunning { get; set; }

        public static void Reset()
        {
            CurrentVersion = 0;
            IsOddsFeedRunning = false;
        }
    }
}
