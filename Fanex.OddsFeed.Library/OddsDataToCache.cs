// --------------------------------------------------------------------------
// <copyright file="OddsDataToCache.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.04.03">Created</change>
// </history>

namespace Fanex.OddsFeed.Library
{
    using System;
    using System.Collections.Generic;
    using Fanex.OddsFeed.Library.Matchs;
    using Fanex.OddsFeed.Library.Outrights;

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
