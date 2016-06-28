// --------------------------------------------------------------------------
// <copyright file="Downtime.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.10.29">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model.ValueObject
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Class Downtime.
    /// </summary>
    public class Downtime
    {
        public string From { get; set; }

        public string To { get; set; }

        public bool IsDowntime { get; set; }
    }
}