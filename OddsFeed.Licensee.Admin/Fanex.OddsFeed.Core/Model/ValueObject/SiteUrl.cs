// --------------------------------------------------------------------------
// <copyright file="SiteUrl.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.10.5">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model.ValueObject
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Class SiteUrl.
    /// </summary>
    public class SiteUrl
    {
        public int SiteUrlId { get; set; }

        public string Url { get; set; }

        public string Environment { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        public Site Site { get; set; }
    }
}