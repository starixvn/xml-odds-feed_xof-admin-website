using Fanex.Logging;
using Fanex.OddsFeed.Common.Configurations;
using Fanex.OddsFeed.WindowsServiceLibrary.Services;
using NLog;
using ONEbook.UM.Services;
using Quartz;
using System;

namespace Fanex.OddsFeed.WindowsService
{
    public class OddsFeedJob : IJob
    {
        private static NLog.Logger _logger = LogManager.GetCurrentClassLogger();
        private static int _totalVersion = AppSettings.TotalVersion;
        private IWindowsOddsFeedService _oddsFeedService;
        private IMaintenanceService _maintenanceService;

        public void Execute(IJobExecutionContext context)
        {
            var startTime = DateTime.Now;
            var isSuccessful = false;
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            _oddsFeedService = (IWindowsOddsFeedService)dataMap.Get("oddsFeedService");
            _maintenanceService = (IMaintenanceService)dataMap.Get("maintenanceService");

            if (OddsFeedStatus.IsOddsFeedRunning || _maintenanceService.IsUnderMaintenance())
            {
                _logger.Trace("Running: " + OddsFeedStatus.IsOddsFeedRunning);
                return;
            }

            try
            {
                OddsFeedStatus.IsOddsFeedRunning = true;
                isSuccessful = _oddsFeedService.Update(OddsFeedStatus.CurrentVersion);
                if (isSuccessful)
                {
                    OddsFeedStatus.CurrentVersion++;
                    UpdateCurrentVersion();
                }
                else
                {
                    LogService.Error("IP address seems to have changed OR Cache on CouchBase is full. Please restart OddsFeedWindowService!");
                }
            }
            catch (Exception exception)
            {
                LogService.Error("BackgroundTimerElapsed -- " + GetExceptionMessage(exception), exception);
            }
            finally
            {
                _logger.Trace("Time: " + (DateTime.Now - startTime) + ", Success: " + isSuccessful);
                OddsFeedStatus.IsOddsFeedRunning = false;
            }
        }

        private void UpdateCurrentVersion()
        {
            if (OddsFeedStatus.CurrentVersion == _totalVersion)
            {
                OddsFeedStatus.CurrentVersion = 0;
                _oddsFeedService.StoreFullChain();
            }
        }

        private static string GetExceptionMessage(Exception exception)
        {
            var messages = new string[]
            {
                "ENVIRONMENT: " + AppSettings.Environment,
                "ERROR: " + exception.Message,
                "SOURCE: " + exception.Source,
                "TARGETSITE: " + exception.TargetSite,
                "STACKTRACE: " + exception.StackTrace
            };

            return string.Join(Environment.NewLine, messages);
        }
    }
}
