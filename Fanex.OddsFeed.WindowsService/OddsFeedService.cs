namespace Fanex.OddsFeed.WindowsService
{
    using System;
    using System.Globalization;
    using System.ServiceProcess;
    using System.Timers;
    using Common.Configurations;
    using Logging;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;
    using WindowsServiceLibrary.Services;

    public partial class OddsFeedService : ServiceBase
    {
        private Timer _timer;
        private int _currentVersion;
        private readonly int _totalVersion;

        private readonly IWindowsOddsFeedService _oddsFeedService;
        private readonly IMaintenanceService _maintenanceService;

        public OddsFeedService()
        {
            InitializeComponent();

            _totalVersion = AppSettings.TotalVersion;

            _oddsFeedService = new WindowsOddsFeedService();
            _maintenanceService = new MaintenanceService();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Reset();

                _timer = new Timer
                {
                    Interval = AppSettings.Interval,
                    Enabled = true
                };

                _timer.Elapsed += BackgroundTimerElapsed;

                _timer.Start();
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
                if (_timer == null)
                {
                    _timer.Enabled = false;
                    _timer.Stop();
                    _timer.Dispose();
                }
                if (_oddsFeedService != null)
                {
                    _oddsFeedService.CloseCouchbaseClient();
                }
            }
            catch (Exception exception)
            {
                LogService.Error("OnStop -- " + GetExceptionMessage(exception));
            }
        }

        private void BackgroundTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var isUnderMaintenance = false;
            try
            {
                _timer.Stop();

                var isSuccessful = _oddsFeedService.Update(_currentVersion);

                if (isSuccessful)
                {
                    _currentVersion++;
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
                isUnderMaintenance = _maintenanceService.IsUnderMaintenance();
            }
            finally
            {
                if (isUnderMaintenance)
                {
                    var underMaintenanceTime = _maintenanceService.GetUnderMaintenanceTime();
                    var umFrom = DateTime.ParseExact(underMaintenanceTime.From, "HH:mm", CultureInfo.InvariantCulture);
                    var umTo = DateTime.ParseExact(underMaintenanceTime.To, "HH:mm", CultureInfo.InvariantCulture);
                    System.Threading.Thread.Sleep(umTo - umFrom);
                }
                _timer.Start();
            }

        }

        private void Reset()
        {
            _currentVersion = 0;

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

        private void UpdateCurrentVersion()
        {
            if (_currentVersion == _totalVersion)
            {
                _currentVersion = 0;
                _oddsFeedService.StoreFullChain();
            }
        }
    }
}