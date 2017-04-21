namespace Fanex.OddsFeed.WindowsService
{
    using Common.Configurations;
    using Logging;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;
    using System;
    using System.ServiceProcess;
    using WindowsServiceLibrary.Services;

    public partial class OddsFeedService : ServiceBase
    {
        private readonly IWindowsOddsFeedService _oddsFeedService;
        private readonly IMaintenanceService _maintenanceService;
        private readonly JobScheduler _scheduler;

        public OddsFeedService()
        {
            InitializeComponent();
            _oddsFeedService = new WindowsOddsFeedService();
            _maintenanceService = new MaintenanceService();
            _scheduler = new JobScheduler();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Reset();
                _scheduler.Start(_oddsFeedService, _maintenanceService);
            }
            catch (Exception exception)
            {
                LogService.Error("OnStart -- " + GetExceptionMessage(exception));
            }
        }

        protected override void OnStop()
        {
            try
            {
                _scheduler.Stop();
                _oddsFeedService.CloseCouchbaseClient();
            }
            catch (Exception exception)
            {
                LogService.Error("OnStop -- " + GetExceptionMessage(exception));
            }
        }

        private void Reset()
        {
            OddsFeedStatus.Reset();
            if (!_maintenanceService.IsUnderMaintenance())
            {
                _oddsFeedService.Reset();
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