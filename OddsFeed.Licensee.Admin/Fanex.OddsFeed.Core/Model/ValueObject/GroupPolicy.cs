// --------------------------------------------------------------------------
// <copyright file="GroupPolicy.cs" company=" Solutions Vietnam">
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
    /// Class GroupPolicy.
    /// </summary>
    public class GroupPolicy
    {
        public int Policy { get; set; }

        public Group Group { get; set; }

        public User Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public User Modifier { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
