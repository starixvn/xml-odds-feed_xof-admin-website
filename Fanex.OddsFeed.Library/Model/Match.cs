// --------------------------------------------------------------------------
// <copyright file="Match.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Library.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class Match.
    /// </summary>
    [Serializable]
    public class Match : BaseModel
    {     
        public Match() : base("Matches")
        {                        
        }
        
        public int MatchId { get; set; }
        
        public int LeagueId { get; set; }

        public string Home { get; set; }

        public string Away { get; set; }

        public string EventDate { get; set; }

        public string KickOffTime { get; set; }
        
        public string Status { get; set; }

        public string SportName { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }

        public int LiveHomeScore { get; set; }

        public int LiveAwayScore { get; set; }
    }
}
