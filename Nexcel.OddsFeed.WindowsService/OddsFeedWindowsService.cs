// --------------------------------------------------------------------------
// <copyright file="OddsFeedWindowsService.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>k

namespace Nexcel.OddsFeed.WindowsService
{
    using System;
    using System.Configuration;
    using System.ServiceProcess;
    using Fanex.Logging;
    using Nexcel.OddsFeed.Library;

    /// <summary>
    /// OddsFeed WindowsService.
    /// </summary>    
    public partial class OddsFeedWindowsService : ServiceBase
    {
        private System.Timers.Timer _timer;
        private int _currentVersion;
        private int _totalVersion;
        private bool _updateSuccessful;

        public OddsFeedWindowsService()
        {
            InitializeComponent();
            Fanex.Data.Configuration.DbSettingProviderManager.Current.Start();
            _totalVersion = int.Parse(ConfigurationManager.AppSettings["TotalVersion"]);
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM)
        /// or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            try
            {
                Reset();

                _timer = new System.Timers.Timer
                {
                    Interval = int.Parse(ConfigurationManager.AppSettings["Interval"]),
                    Enabled = true
                };

                _timer.Elapsed += BackgroundTimerElapsed;

                _timer.Start();
            }
            catch (Exception ex)
            {
                string startTime = string.Empty;
                string endTime = string.Empty;
                if (!Utilities.GetDownTime(out startTime, out endTime))
                {
                    string[] message = new string[]
                    {
                        "ENVIRONMENT: " + ConfigurationManager.AppSettings["Environment"].ToString(),
                        "ERROR: " + ex.Message, 
                        "SOURCE: " + ex.Source,
                        "TARGETSITE: " + ex.TargetSite,
                        "STACKTRACE: " + ex.StackTrace
                    };
                    LogService.Error("OnStart -- " + string.Join(Environment.NewLine, message));
                }
            }
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM).
        /// Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                _timer.Enabled = false;

                Reset();

                _timer.Stop();
            }
            catch (Exception ex)
            {
                string startTime = string.Empty;
                string endTime = string.Empty;
                if (!Utilities.GetDownTime(out startTime, out endTime))
                {
                    string[] message = new string[]
                    {
                        "ENVIRONMENT: " + ConfigurationManager.AppSettings["Environment"].ToString(),
                        "ERROR: " + ex.Message, 
                        "SOURCE: " + ex.Source,
                        "TARGETSITE: " + ex.TargetSite,
                        "STACKTRACE: " + ex.StackTrace
                    };
                    LogService.Error("OnStop -- " + string.Join(Environment.NewLine, message));
                }
            }
        }

        /// <summary>
        /// Backgrounds the timer elapsed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        private void BackgroundTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();
            string startTime = string.Empty;
            string endTime = string.Empty;

            try
            {
                if (!Utilities.GetDownTime(out startTime, out endTime))
                {
                    _updateSuccessful = UpdateOddsData.Update(_currentVersion);
                }

                _currentVersion = _updateSuccessful == true ? _currentVersion + 1 : _currentVersion;
                if (_currentVersion == _totalVersion)
                {
                    _currentVersion = 0;

                    if (!Utilities.GetDownTime(out startTime, out endTime))
                    {
                        UpdateOddsData.FullChain();
                    }
                }

                if (_updateSuccessful)
                {
                    if (!Utilities.GetDownTime(out startTime, out endTime))
                    {
                        _updateSuccessful = false || UpdateOddsData.Update(_currentVersion);
                    }

                    if (_updateSuccessful == false)
                    {
                        LogService.Error("IP address seems to have changed OR Cache on CouchBase is full. Please restart OddsFeedWindowService ! ");

                        CouchbaseManager.Instance.FlushAll();
                        CouchbaseManager.Instance.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                if (!Utilities.GetDownTime(out startTime, out endTime))
                {
                    string[] message = new string[]
                    {
                        "ENVIRONMENT: " + ConfigurationManager.AppSettings["Environment"].ToString(),
                        "ERROR: " + ex.Message, 
                        "SOURCE: " + ex.Source,
                        "TARGETSITE: " + ex.TargetSite,
                        "STACKTRACE: " + ex.StackTrace
                    };
                    LogService.Error("BackgroundTimerElapsed -- " + string.Join(Environment.NewLine, message));
                }
            }

            _timer.Start();
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        private void Reset()
        {
            try
            {
                _updateSuccessful = false;
                _currentVersion = 0;

                string startTime = string.Empty;
                string endTime = string.Empty;

                if (!Utilities.GetDownTime(out startTime, out endTime))
                {
                    UpdateOddsData.Reset();
                }
            }
            catch (Exception ex)
            {
                string startTime = string.Empty;
                string endTime = string.Empty;
                if (!Utilities.GetDownTime(out startTime, out endTime))
                {
                    string[] message = new string[]
                    {
                        "ENVIRONMENT: " + ConfigurationManager.AppSettings["Environment"].ToString(),
                        "ERROR: " + ex.Message, 
                        "SOURCE: " + ex.Source,
                        "TARGETSITE: " + ex.TargetSite,
                        "STACKTRACE: " + ex.StackTrace
                    };
                    LogService.Error("Reset -- " + string.Join(Environment.NewLine, message));
                }
            }
        }
    }
}
