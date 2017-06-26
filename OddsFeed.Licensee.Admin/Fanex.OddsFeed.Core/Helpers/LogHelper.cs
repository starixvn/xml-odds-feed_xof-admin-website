namespace Fanex.OddsFeed.Core.Helpers
{
    using System;
    using Core.Configuration;
    using Logging;

    public static class LogHelper
    {
        public static void Log(Exception exception)
        {
            var message = new[]
            {
                            "ENVIRONMENT: " + NapSettings.Settings.Environment,
                            "ERROR: " + exception.Message,
                            "SOURCE: " + exception.Source,
                            "TARGETSITE: " + exception.TargetSite,
                            "STACKTRACE: " + exception.StackTrace
                };

            LogService.Error(string.Join(Environment.NewLine, message), exception);
        }
    }
}