// --------------------------------------------------------------------------
// <copyright file="League.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Nexcel.OddsFeed.Library.Matchs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class League.
    /// </summary>
    public class League
    {
        public int LeagueId { get; set; }

        public List<Match> Matches { get; set; }
    }
}
