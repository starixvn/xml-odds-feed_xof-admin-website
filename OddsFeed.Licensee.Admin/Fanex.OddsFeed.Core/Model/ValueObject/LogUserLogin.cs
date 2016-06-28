// --------------------------------------------------------------------------
// <copyright file="LogUserLogin.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.11.14">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model.ValueObject
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Class LogUserLogin.
    /// </summary>
    public class LogUserLogin
    {
        public long Id { get; set; }
               
        public string IP { get; set; }

        public DateTime LogDate { get; set; }

        public string Doing { get; set; }

        public string DomainName { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }

        public string CountryCode { get; set; }

        public int Duration { get; set; }

        public User User { get; set; }
    }
}
