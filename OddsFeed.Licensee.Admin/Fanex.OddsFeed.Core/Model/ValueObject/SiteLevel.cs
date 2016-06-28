// --------------------------------------------------------------------------
// <copyright file="SiteLevel.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.8">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model.ValueObject
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Class SiteLevel.
    /// </summary>
    public class SiteLevel
    {
        public Site Site { get; set; }

        public int Level { get; set; }

        public bool IsActive { get; set; }
        
        public DateTime LastLoginDate { get; set; }

        public User Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public User Modifier { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}