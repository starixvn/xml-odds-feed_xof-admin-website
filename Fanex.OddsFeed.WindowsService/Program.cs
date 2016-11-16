// --------------------------------------------------------------------------
// <copyright file="Program.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.WindowsService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using Fanex.OddsFeed.Library;

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
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[] 
            { 
                new OddsFeedService()
            };
            ServiceBase.Run(servicesToRun);
            //// Fanex.Data.Configuration.DbSettingProviderManager.Current.Start();
            //// UpdateOddsData.Update(10);
        }
    }
}
