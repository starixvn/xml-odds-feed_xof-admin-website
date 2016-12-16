namespace Fanex.OddsFeed.WindowsService
{
    using System.ServiceProcess;
    using Data.Configuration;

    /// <summary>
    /// Class Program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            DbSettingProviderManager.Current.Start();
            using (var feedService = new OddsFeedService())
            {
                ServiceBase[] servicesToRun;
                servicesToRun = new ServiceBase[]
                {
                feedService
                };
                ServiceBase.Run(servicesToRun);

            }
        }
    }
}
