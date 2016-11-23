namespace Fanex.OddsFeed.WindowsService
{
    using System;
    using System.ServiceProcess;
    using System.Timers;
    using Couchbase;
    using Fanex.Logging;
    using Fanex.OddsFeed.Library;
    using Library.Configurations;
    using Library.Services;

    public partial class OddsFeedService : ServiceBase
    {
        private Timer _timer;
        private int _currentVersion;
        private int _totalVersion;

        private IWindowsOddsFeedService _oddsFeedService;

        public OddsFeedService()
        {
            InitializeComponent();

            _totalVersion = AppSettings.TotalVersion;

            _oddsFeedService = new WindowsOddsFeedService();
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

                Reset();

            }
            catch (Exception exception)
            {
                LogService.Error("OnStop -- " + GetExceptionMessage(exception));
            }
        }

        private void BackgroundTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();

            try
            {
                if (!Utilities.IsDownTime())
                {
                    var isSuccessful = _oddsFeedService.Update(_currentVersion);

                    if (isSuccessful)
                    {
                        _currentVersion++;
                        UpdateCurrentVersion();
                    }
                    else
                    {
                        LogService.Error("IP address seems to have changed OR Cache on CouchBase is full. Please restart OddsFeedWindowService ! ");

                    }
                }
            }
            catch (Exception exception)
            {
                LogService.Error("BackgroundTimerElapsed -- " + GetExceptionMessage(exception));
            }

            _timer.Start();
        }

        private void Reset()
        {
            _currentVersion = 0;

            if (!Utilities.IsDownTime())
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