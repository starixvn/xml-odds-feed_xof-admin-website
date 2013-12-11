// --------------------------------------------------------------------------
// <copyright file="OddsDataToCache.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.04.03">Created</change>
// </history>

namespace Nexcel.OddsFeed.Library
{
    using System;
    using System.Collections.Generic;
    using Nexcel.OddsFeed.Library.Matchs;
    using Nexcel.OddsFeed.Library.Outrights;

    /// <summary>
    /// Class OddsDataToCache.
    /// </summary>
    [Serializable]
    public class OddsDataToCache
    {
        public IList<Match> LiveMatches { get; set; }

        public IList<Match> DeadballMatches { get; set; }

        public IList<Event> Events { get; set; }
    }
}
